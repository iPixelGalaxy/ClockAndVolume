using UnityEngine;

namespace ClockAndVolume.Volume
{
    public abstract class AudioHelpers
    {
        public static float NormalizedVolumeToDB(float normalizedVolume)
        {
            return Mathf.Max(-100f, Mathf.Log(normalizedVolume, 1.1f));
        }

        public static float DBToNormalizedVolume(float db)
        {
            return Mathf.Pow(1.1f, db);
        }
    }
}
