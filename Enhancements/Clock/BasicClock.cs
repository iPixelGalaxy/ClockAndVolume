﻿using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using BeatSaberMarkupLanguage.FloatingScreen;

namespace Enhancements.Clock
{
    public class BasicClock : IInitializable, IDisposable
    {
        private bool _disabled;
        private XLoader _loader;
        private FloatingScreen _floatingScreen;
        private readonly ClockSettings _clockSettings;
        private readonly BasicClockView _basicClockView;
        private readonly IClockController _clockController;

        public BasicClock(XLoader loader, ClockSettings clockSettings, BasicClockView basicClockView, IClockController clockController)
        {
            _loader = loader;
            _clockSettings = clockSettings;
            _basicClockView = basicClockView;
            _clockController = clockController;
        }

        public void Initialize()
        {
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(150f, 50f), false, new Vector3(0f, 2.8f, 2.45f), Quaternion.Euler(new Vector3(325f, 0f, 0f)));
            _floatingScreen.GetComponent<Image>().enabled = false;
            _floatingScreen.SetRootViewController(_basicClockView, false);

            _disabled = !_clockSettings.Enabled;
            _clockSettings.MarkDirty();
            ClockController_DateUpdated(DateTime.Now);
            _clockController.DateUpdated += ClockController_DateUpdated;
        }

        public void Dispose()
        {
            _clockController.DateUpdated -= ClockController_DateUpdated;
        }

        private void ClockController_DateUpdated(DateTime time)
        {
            if (_disabled && !_clockSettings.Enabled)
            {
                return;
            }
            else if (_disabled && _clockSettings.Enabled)
            {
                _floatingScreen.gameObject.SetActive(true);
                _disabled = false;
            }
            CultureInfo culture = string.IsNullOrEmpty(_clockSettings.Culture) ? CultureInfo.InvariantCulture : new CultureInfo(_clockSettings.Culture);
            if (_clockSettings.Enabled)
            {
                _basicClockView.ClockText = time.ToString(_clockSettings.Format, culture);
                if (_clockSettings.IsDirty)
                {
                    _basicClockView.ClockSize = _clockSettings.Size;
                    _basicClockView.Font = _loader.GetFont(_clockSettings.Font);
                    _basicClockView.ClockColor = _clockSettings.Color.ColorWithAlpha(_clockSettings.Opacity);
                    _clockSettings.IsDirty = false;
                }
            }
            else
            {
                _disabled = true;
                _basicClockView.ClockText = "";
                _floatingScreen.gameObject.SetActive(false);
            }
        }
    }
}