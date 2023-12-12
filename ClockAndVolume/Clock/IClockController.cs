using System;

namespace ClockAndVolume.Clock
{
    public interface IClockController
    {
        event Action<DateTime> DateUpdated;
        DateTime GetCurrentTime();
    }
}