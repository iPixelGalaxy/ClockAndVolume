using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ClockAndVolume
{
    internal static class LegacyConfigFileWorker
    {
        private sealed class Request
        {
            private readonly string currentPath;
            private readonly string legacyPath;
            internal readonly TaskCompletionSource<bool> Completion =
                new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            internal Request(string currentPath, string legacyPath)
            {
                this.currentPath = currentPath;
                this.legacyPath = legacyPath;
            }

            internal void Execute()
            {
                try
                {
                    if (!File.Exists(currentPath) && File.Exists(legacyPath))
                        File.Copy(legacyPath, currentPath);
                    Completion.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    Completion.TrySetException(ex);
                }
            }
        }

        private static readonly object gate = new object();
        private static readonly Queue<Request> requests = new Queue<Request>();
        private static Task worker;

        internal static void Migrate(string currentPath, string legacyPath)
        {
            var request = new Request(currentPath, legacyPath);
            lock (gate)
            {
                requests.Enqueue(request);
                if (worker == null)
                    StartWorker();
            }

            Task<bool> completion = request.Completion.Task;
            // Observe physical completion before retrieving the result on this caller.
            if (!completion.IsCompleted)
                ((IAsyncResult)completion).AsyncWaitHandle.WaitOne();
            completion.GetAwaiter().GetResult();
        }

        private static void StartWorker()
        {
            if (ExecutionContext.IsFlowSuppressed())
            {
                ScheduleWorker();
                return;
            }

            using (ExecutionContext.SuppressFlow())
                ScheduleWorker();
        }

        private static void ScheduleWorker()
        {
            worker = Task.Factory.StartNew(Drain, CancellationToken.None,
                TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            worker.ContinueWith(WorkerCompleted, CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        private static void Drain()
        {
            while (true)
            {
                Request request;
                lock (gate)
                {
                    if (requests.Count == 0)
                        return;
                    request = requests.Dequeue();
                }
                request.Execute();
            }
        }

        private static void WorkerCompleted(Task completed)
        {
            lock (gate)
            {
                if (!ReferenceEquals(worker, completed))
                    return;
                worker = null;
                if (requests.Count != 0)
                    StartWorker();
            }
        }
    }
}
