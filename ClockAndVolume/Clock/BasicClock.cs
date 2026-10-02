using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using BeatSaberMarkupLanguage.FloatingScreen;
using VRUIControls;
using IPA.Utilities;

namespace ClockAndVolume.Clock
{
    public class BasicClock : IInitializable, IDisposable
    {
        private bool _disabled;
        private readonly ClockTextFormatter _textFormatter;
        private XLoader _loader;
        private FloatingScreen _floatingScreen;
        private readonly ClockSettings _clockSettings;
        private readonly BasicClockView _basicClockView;
        private readonly IClockController _clockController;
        private readonly PhysicsRaycasterWithCache _physicsRaycasterWithCache;

        public BasicClock(XLoader loader, ClockSettings clockSettings, BasicClockView basicClockView, IClockController clockController, PhysicsRaycasterWithCache physicsRaycasterWithCache)
        {
            _loader = loader;
            _clockSettings = clockSettings;
            _basicClockView = basicClockView;
            _clockController = clockController;
            _physicsRaycasterWithCache = physicsRaycasterWithCache;
            _textFormatter = new ClockTextFormatter(PublishClockText);
        }

        public void Initialize()
        {
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(150f, 50f), false, _clockSettings.Position, Quaternion.Euler(_clockSettings.Rotation));
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
            _textFormatter.Dispose();
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
            if (_clockSettings.Enabled)
            {
                _textFormatter.Update(time, _clockSettings.Format, _clockSettings.Culture);
                if (_clockSettings.IsDirty)
                {
                    _basicClockView.ClockSize = _clockSettings.Size;
                    _basicClockView.ClockColor = new Color (_clockSettings.Color.r, _clockSettings.Color.g, _clockSettings.Color.b, _clockSettings.Opacity);
                    _floatingScreen.ScreenPosition = _clockSettings.Position;
                    _floatingScreen.ScreenRotation = Quaternion.Euler(_clockSettings.Rotation);
                    _clockSettings.IsDirty = false;
                }
            }
            else
            {
                _disabled = true;
                _textFormatter.Invalidate();
                _basicClockView.ClockText = "";
                _floatingScreen.gameObject.SetActive(false);
            }
        }

        private void PublishClockText(string text, string format, string culture)
        {
            if (_clockSettings.Enabled && _clockSettings.Format == format && _clockSettings.Culture == culture
                && _floatingScreen && _basicClockView)
                _basicClockView.ClockText = text;
        }
    }
}
