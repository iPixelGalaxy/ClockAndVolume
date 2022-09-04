using HMUI;
using Zenject;
using ClockAndVolume.UI.Clock;
using ClockAndVolume.UI.Credits;
using ClockAndVolume.UI.Volume;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.FloatingScreen;
using System;
using VRUIControls;
using IPA.Utilities;
using UnityEngine;
using ClockAndVolume.Clock;

namespace ClockAndVolume.UI
{
    public class XSettingsFlowCoordinator : FlowCoordinator
    {
        private Config _config;
        private FloatingScreen _floatingScreen;
        private ClockSettings _clockSettings;
        private BasicClockViewPreview _basicClockViewPreview;
        private MainFlowCoordinator _mainFlowCoordinator;

        private ClockSettingsView _clockSettingsView;
        private CreditsInfoView _creditsInfoView;
        private VolumeSettingsInfoView _volumeSettingsInfoView;

        [Inject]
        public void Construct(Config config, MainFlowCoordinator mainFlowCoordinator, BasicClockViewPreview basicClockViewPreview, ClockSettingsView clockSettingsView, CreditsInfoView creditsInfoView, VolumeSettingsInfoView volumeSettingsInfoView)
        {
            _config = config;
            _mainFlowCoordinator = mainFlowCoordinator;
            _clockSettingsView = clockSettingsView;
            _creditsInfoView = creditsInfoView;
            _volumeSettingsInfoView = volumeSettingsInfoView;
            _basicClockViewPreview = basicClockViewPreview;
        }
        public void Initialize()
        {
            _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(150f, 50f), false, _clockSettings.PositionGame, Quaternion.Euler(_clockSettings.RotationGame));
            _floatingScreen.SetRootViewController(_basicClockViewPreview, HMUI.ViewController.AnimationType.Out);

        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                showBackButton = true;
                SetTitle("Clock and Volume");
                ProvideInitialViewControllers(_clockSettingsView, _volumeSettingsInfoView, _creditsInfoView);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            _config.Changed();
            base.BackButtonWasPressed(topViewController);
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}