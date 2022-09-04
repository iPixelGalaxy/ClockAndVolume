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

namespace ClockAndVolume.UI.Clock
{
    [ViewDefinition("ClockAndVolume.Views.clock-settings-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\..\Views\clock-settings-view.bsml")]
    public class ClockSettingsView : BSMLAutomaticViewController
    {
        [UIValue("format-options")]
        public List<object> formatOptions = new List<object>();

        [UIValue("show-ingame")]
        protected bool ShowInGame
        {
            get => _settings.ShowInGame;
            set => _settings.ShowInGame = value;
        }

        [UIValue("size")]
        protected int Size
        {
            get => (int)_settings.Size;
            set => _settings.Size = value;
        }

        [UIValue("format")]
        protected string Format
        {
            get => _settings.Format;
            set => _settings.Format = value;
        }


        [UIParams]
        protected BSMLParserParams parserParams;

        [UIComponent("dropdown")]
        protected DropDownListSetting dropdown;

        private ClockSettings _settings;

        [Inject]
        public void Construct(XLoader loader, ClockSettings settings)
        {
            _settings = settings;
            ReloadFormats();
        }

        protected void ReloadFormats()
        {
            formatOptions.Clear();
            formatOptions.AddRange(_settings.Formats);
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
            get => _settings.Enabled;
            set => _settings.Enabled = value;
        }

        [UIValue("pos-x")]
        protected float PosX
        {
            get => _settings.Position.x;
            set => _settings.Position = new Vector3(value, _settings.Position.y, _settings.Position.z);
        }

        [UIValue("pos-y")]
        protected float PosY
        {
            get => _settings.Position.y;
            set => _settings.Position = new Vector3(_settings.Position.x, value, _settings.Position.z);
        }

        [UIValue("pos-z")]
        protected float PosZ
        {
            get => _settings.Position.z;
            set => _settings.Position = new Vector3(_settings.Position.x, _settings.Position.y, value);
        }

        [UIValue("rot-x")]
        protected float RotX
        {
            get => _settings.Rotation.x;
            set => _settings.Rotation = new Vector3(value, _settings.Rotation.y, _settings.Rotation.z);
        }

        [UIValue("rot-y")]
        protected float RotY
        {
            get => _settings.Rotation.y;
            set => _settings.Rotation = new Vector3(_settings.Rotation.x, value, _settings.Rotation.z);
        }


        [UIValue("rot-z")]
        protected float RotZ
        {
            get => _settings.Rotation.z;
            set => _settings.Rotation = new Vector3(_settings.Rotation.x, _settings.Rotation.y, value);
        }

        [UIValue("pos-x-game")]
        protected float PosXGame
        {
            get => _settings.PositionGame.x;
            set => _settings.PositionGame = new Vector3(value, _settings.PositionGame.y, _settings.PositionGame.z);
        }

        [UIValue("pos-y-game")]
        protected float PosYGame
        {
            get => _settings.PositionGame.y;
            set => _settings.PositionGame = new Vector3(_settings.PositionGame.x, value, _settings.PositionGame.z);
        }

        [UIValue("pos-z-game")]
        protected float PosZGame
        {
            get => _settings.PositionGame.z;
            set => _settings.PositionGame = new Vector3(_settings.PositionGame.x, _settings.PositionGame.y, value);
        }

        [UIValue("rot-x-game")]
        protected float RotXGame
        {
            get => _settings.RotationGame.x;
            set => _settings.RotationGame = new Vector3(value, _settings.RotationGame.y, _settings.RotationGame.z);
        }

        [UIValue("rot-y-game")]
        protected float RotYGame
        {
            get => _settings.RotationGame.y;
            set => _settings.RotationGame = new Vector3(_settings.RotationGame.x, value, _settings.RotationGame.z);
        }


        [UIValue("rot-z-game")]
        protected float RotZGame
        {
            get => _settings.RotationGame.z;
            set => _settings.RotationGame = new Vector3(_settings.RotationGame.x, _settings.RotationGame.y, value);
        }

        [UIValue("color")]
        protected Color Color
        {
            get => _settings.Color;
            set => _settings.Color = value;
        }

        void ResetClockColor()
        {
            Color = new Color(1, 1, 1, 1);
            NotifyPropertyChanged("Color");
        }

        [UIValue("opacity")]
        protected float Opacity
        {
            get => _settings.Opacity;
            set => _settings.Opacity = value;
        }
    }
}