using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using BeatSaberMarkupLanguage.FloatingScreen;
using VRUIControls;
using IPA.Utilities;

namespace ClockAndVolume.Clock
{
    public class GameClock : IInitializable, IDisposable
    {
        private bool _disabled;
        private XLoader _loader;
        private FloatingScreen _floatingScreen;
        private readonly ClockSettings _clockSettings;
        private readonly BasicClockView _basicClockView;
        private readonly IClockController _clockController;
        private readonly PhysicsRaycasterWithCache _physicsRaycasterWithCache;

        public GameClock(XLoader loader, ClockSettings clockSettings, BasicClockView basicClockView, IClockController clockController, PhysicsRaycasterWithCache physicsRaycasterWithCache)
        {
            _loader = loader;
            _clockSettings = clockSettings;
            _basicClockView = basicClockView;
            _clockController = clockController;
            _physicsRaycasterWithCache = physicsRaycasterWithCache;
        }

        public void Initialize()
        {
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(150f, 50f), false, _clockSettings.PositionGame, Quaternion.Euler(_clockSettings.RotationGame));
            _floatingScreen.GetComponent<VRGraphicRaycaster>().SetField("_physicsRaycaster", _physicsRaycasterWithCache);
            //_floatingScreen.GetComponent<Image>().enabled = false;
            _floatingScreen.SetRootViewController(_basicClockView, HMUI.ViewController.AnimationType.Out);

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
                    _basicClockView.ClockColor = new Color(_clockSettings.Color.r, _clockSettings.Color.g, _clockSettings.Opacity);
                    _floatingScreen.ScreenPosition = _clockSettings.PositionGame;
                    _floatingScreen.ScreenRotation = Quaternion.Euler(_clockSettings.RotationGame);
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