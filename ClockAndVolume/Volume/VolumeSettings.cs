﻿namespace ClockAndVolume.Volume
{
    public class VolumeSettings
    {
        public virtual float GoodCuts { get; set; } = 1f;
        public virtual float BadCuts { get; set; } = 1f;
        public virtual float Music { get; set; } = 1f;
        public virtual float SongPreview { get; set; } = 1f;
        public virtual float MenuBackground { get; set; } = 1f;
        public virtual float Fireworks { get; set; } = 1f;
    }
}