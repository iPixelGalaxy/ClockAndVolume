using ClockAndVolume.Clock;
using ClockAndVolume.Volume;
using IPA.Config.Stores.Attributes;
using SiraUtil.Converters;
using System.IO;
using Version = Hive.Versioning.Version;

namespace ClockAndVolume
{
    public class Config
    {
        internal static Config Value;
        public virtual ClockSettings Clock { get; set; } = new ClockSettings();
        [NonNullable]
        public virtual VolumeSettings Volume { get; set; } = new VolumeSettings();

        public virtual void Changed()
        {
            Clock.MarkDirty();
        }
    }
}