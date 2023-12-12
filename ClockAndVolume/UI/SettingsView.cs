using System;
using Zenject;
using System.Linq;
using ClockAndVolume.Clock;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components.Settings;
using UnityEngine;
using ClockAndVolume.Volume;
using IPA.Utilities;
using System.Diagnostics;

namespace ClockAndVolume.UI
{
    [ViewDefinition("ClockAndVolume.Views.settings-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\settings-view.bsml")]
    public class SettingsView : BSMLAutomaticViewController
    {

        [UIValue("format-options")]
        public List<object> formatOptions = new List<object>();

        [UIValue("show-ingame")]
        protected bool ShowInGame
        {
            get => _clockSettings.ShowInGame;
            set => _clockSettings.ShowInGame = value;
        }

        [UIValue("size")]
        protected int Size
        {
            get => (int)_clockSettings.Size;
            set => _clockSettings.Size = value;
        }

        [UIValue("format")]
        protected string Format
        {
            get => _clockSettings.Format;
            set => _clockSettings.Format = value;
        }


        [UIParams]
        protected BSMLParserParams parserParams;

        [UIComponent("dropdown")]
        protected DropDownListSetting dropdown;

        private ClockSettings _clockSettings;

        [Inject]
        public void Construct(XLoader loader, ClockSettings settings)
        {
            _clockSettings = settings;
            ReloadFormats();
        }

        protected void ReloadFormats()
        {
            formatOptions.Clear();
            formatOptions.AddRange(_clockSettings.Formats);
            dropdown?.UpdateChoices();
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            parserParams.EmitEvent("hide-keyboard");
        }

        [UIAction("formatter-formatter")]
        protected string FormatterFormatter(string format)
        {
            return DateTime.Now.ToString(format);
        }

        [UIValue("enabled")]
        protected bool Enabled
        {
            get => _clockSettings.Enabled;
            set => _clockSettings.Enabled = value;
        }

        [UIValue("pos-x")]
        protected float PosX
        {
            get => _clockSettings.Position.x;
            set => _clockSettings.Position = new Vector3(value, _clockSettings.Position.y, _clockSettings.Position.z);
        }

        [UIValue("pos-y")]
        protected float PosY
        {
            get => _clockSettings.Position.y;
            set => _clockSettings.Position = new Vector3(_clockSettings.Position.x, value, _clockSettings.Position.z);
        }

        [UIValue("pos-z")]
        protected float PosZ
        {
            get => _clockSettings.Position.z;
            set => _clockSettings.Position = new Vector3(_clockSettings.Position.x, _clockSettings.Position.y, value);
        }

        [UIValue("rot-x")]
        protected float RotX
        {
            get => _clockSettings.Rotation.x;
            set => _clockSettings.Rotation = new Vector3(value, _clockSettings.Rotation.y, _clockSettings.Rotation.z);
        }

        [UIValue("rot-y")]
        protected float RotY
        {
            get => _clockSettings.Rotation.y;
            set => _clockSettings.Rotation = new Vector3(_clockSettings.Rotation.x, value, _clockSettings.Rotation.z);
        }


        [UIValue("rot-z")]
        protected float RotZ
        {
            get => _clockSettings.Rotation.z;
            set => _clockSettings.Rotation = new Vector3(_clockSettings.Rotation.x, _clockSettings.Rotation.y, value);
        }

        [UIValue("pos-x-game")]
        protected float PosXGame
        {
            get => _clockSettings.PositionGame.x;
            set => _clockSettings.PositionGame = new Vector3(value, _clockSettings.PositionGame.y, _clockSettings.PositionGame.z);
        }

        [UIValue("pos-y-game")]
        protected float PosYGame
        {
            get => _clockSettings.PositionGame.y;
            set => _clockSettings.PositionGame = new Vector3(_clockSettings.PositionGame.x, value, _clockSettings.PositionGame.z);
        }

        [UIValue("pos-z-game")]
        protected float PosZGame
        {
            get => _clockSettings.PositionGame.z;
            set => _clockSettings.PositionGame = new Vector3(_clockSettings.PositionGame.x, _clockSettings.PositionGame.y, value);
        }

        [UIValue("rot-x-game")]
        protected float RotXGame
        {
            get => _clockSettings.RotationGame.x;
            set => _clockSettings.RotationGame = new Vector3(value, _clockSettings.RotationGame.y, _clockSettings.RotationGame.z);
        }

        [UIValue("rot-y-game")]
        protected float RotYGame
        {
            get => _clockSettings.RotationGame.y;
            set => _clockSettings.RotationGame = new Vector3(_clockSettings.RotationGame.x, value, _clockSettings.RotationGame.z);
        }


        [UIValue("rot-z-game")]
        protected float RotZGame
        {
            get => _clockSettings.RotationGame.z;
            set => _clockSettings.RotationGame = new Vector3(_clockSettings.RotationGame.x, _clockSettings.RotationGame.y, value);
        }

        [UIValue("color")]
        protected Color Color
        {
            get => _clockSettings.Color;
            set => _clockSettings.Color = value;
        }

        void ResetClockColor()
        {
            Color = new Color(1, 1, 1, 1);
            NotifyPropertyChanged("Color");
        }

        [UIValue("opacity")]
        protected float Opacity
        {
            get => _clockSettings.Opacity;
            set => _clockSettings.Opacity = value;
        }
        private VolumeSettings _volumeSettings;
        private MenuVolumeManager _menuVolume;
        private FireworksController _fireworksController;
        private FireworkItemController.Pool _fireworkPool;
        private Sprite auros;
        private static readonly FieldAccessor<FireworksController, FireworkItemController.Pool>.Accessor FireworkPool = FieldAccessor<FireworksController, FireworkItemController.Pool>.GetAccessor("_fireworkItemPool");

        [UIValue("good-cut")]
        protected float GoodCut
        {
            get => _volumeSettings.GoodCuts;
            set => _volumeSettings.GoodCuts = value;
        }

        [UIValue("bad-cut")]
        protected float BadCut
        {
            get => _volumeSettings.BadCuts;
            set => _volumeSettings.BadCuts = value;
        }

        [UIValue("music")]
        protected float Music
        {
            get => _volumeSettings.Music;
            set => _volumeSettings.Music = value;
        }

        [UIValue("song-preview")]
        protected float Preview
        {
            get => _volumeSettings.SongPreview;
            set
            {
                _volumeSettings.SongPreview = value;
                _menuVolume.SetMenuPreviewVolume(value);
            }
        }

        [UIValue("background")]
        protected float Background
        {
            get => _volumeSettings.MenuBackground;
            set
            {
                _volumeSettings.MenuBackground = value;
                _menuVolume.SetMenuAmbienceVolume(value);
            }
        }

        [UIValue("fireworks")]
        protected float Fireworks
        {
            get => _volumeSettings.Fireworks;
            set => _volumeSettings.Fireworks = value;
        }

        [Inject]
        public void Construct(VolumeSettings settings, MenuVolumeManager menuVolume, FireworksController fireworkController)
        {
            _volumeSettings = settings;
            _menuVolume = menuVolume;
            _fireworksController = fireworkController;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {

            _fireworkPool = FireworkPool(ref _fireworksController);
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        }

        [UIAction("percent")]
        public string PercentFormat(float value)
        {
            return value.ToString("P");
        }
        [UIAction("#AurosGithub")]
        public void AurosGithub() => OpenGithub("https://github.com/Auros");

        [UIAction("#PixelGithub")]
        public void PixelGithub() => OpenGithub("https://github.com/iPixelGalaxy");
        private void OpenGithub(string url)
        {
            Process.Start(url);
        }
    }
}