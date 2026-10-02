using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using IPA.Utilities.Async;

namespace ClockAndVolume.Clock
{
    internal sealed class ClockTextFormatter : IDisposable
    {
        private sealed class FormatterState
        {
            private string _cultureName;
            private CultureInfo _culture = CultureInfo.InvariantCulture;

            internal string Format(Request request)
            {
                if (_cultureName != request.Culture)
                {
                    _culture = string.IsNullOrEmpty(request.Culture)
                        ? CultureInfo.InvariantCulture : new CultureInfo(request.Culture);
                    _cultureName = request.Culture;
                }
                return request.Time.ToString(request.Format, _culture);
            }
        }

        private sealed class Request
        {
            internal readonly DateTime Time;
            internal readonly string Format;
            internal readonly string Culture;
            internal readonly int Generation;
            internal readonly FormatterState Formatter;

            internal Request(DateTime time, string format, string culture, int generation, FormatterState formatter)
            {
                Time = time;
                Format = format;
                Culture = culture;
                Generation = generation;
                Formatter = formatter;
            }
        }

        private readonly FormatterState _formatter = new FormatterState();
        private Action<string, string, string> _publish;
        private Task<string> _worker;
        private Request _pending;
        private string _format;
        private string _culture;
        private int _generation;
        private bool _disposed;

        internal ClockTextFormatter(Action<string, string, string> publish)
        {
            _publish = publish;
        }

        internal void Update(DateTime time, string format, string culture)
        {
            if (_disposed)
                return;
            if (_format != format || _culture != culture)
            {
                ++_generation;
                _format = format;
                _culture = culture;
            }
            _pending = new Request(time, format, culture, _generation, _formatter);
            StartNext();
        }

        internal void Invalidate()
        {
            ++_generation;
            _pending = null;
        }

        public void Dispose()
        {
            _disposed = true;
            Invalidate();
            _publish = null;
        }

        private void StartNext()
        {
            if (_disposed || _worker != null || _pending == null)
                return;
            Request request = _pending;
            _pending = null;
            _worker = Task.Factory.StartNew(FormatRequest, request, CancellationToken.None,
                TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            _worker.ContinueWith(Completed, CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously, UnityMainThreadTaskScheduler.Default);
        }

        private static string FormatRequest(object state)
        {
            Request request = (Request)state;
            return request.Formatter.Format(request);
        }

        private void Completed(Task<string> task)
        {
            if (!ReferenceEquals(_worker, task))
                return;
            _worker = null;
            Request request = (Request)task.AsyncState;
            try
            {
                if (task.IsFaulted)
                {
                    Exception error = task.Exception.GetBaseException();
                    if (!_disposed && request.Generation == _generation)
                        Plugin.Log.Error($"Unable to format clock text: {error}");
                }
                else if (!_disposed && request.Generation == _generation && _pending == null && !task.IsCanceled)
                {
                    _publish(task.GetAwaiter().GetResult(), request.Format, request.Culture);
                }
            }
            finally
            {
                StartNext();
            }
        }
    }
}
