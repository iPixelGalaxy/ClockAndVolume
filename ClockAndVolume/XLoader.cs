using TMPro;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using IPA.Utilities;

namespace ClockAndVolume
{
    public class XLoader
    {
        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private static readonly Dictionary<string, TMP_FontAsset> _fonts = new Dictionary<string, TMP_FontAsset>();

        public void Initialize()
        {
            if (!File.Exists(Path.Combine(UnityGame.UserDataPath, "ClockAndVolume.json")))
            {
                if (!File.Exists(Path.Combine(UnityGame.UserDataPath, "Enhancements.json")))
                {
                    //make config, idk how you have it set up
                }
                else
                {
                    File.Copy(Path.Combine(UnityGame.UserDataPath, "Enhancements.json"), Path.Combine(UnityGame.UserDataPath, "ClockAndVolume.json"));
                }
            }
        }

        public string[] GetFontNames()
        {
            Initialize();
            return _fonts.Keys.ToArray();
        }
    }
}