using TMPro;
using Zenject;
using UnityEngine;
using ClockAndVolume.Clock;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace ClockAndVolume.Clock
{
    [ViewDefinition("ClockAndVolume.Views.basic-clock.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\basic-clock.bsml")]
    public class BasicClockView : BSMLAutomaticViewController, IInitializable
    {

        [UIComponent("clock-text")]
        protected ClickableText _clockTextObject;

        private string _clockText;
        [UIValue("clock-text")]
        public string ClockText
        {
            get => _clockText;
            set
            {
                _clockText = value;
                NotifyPropertyChanged();
            }
        }

        private float _clockSize = 10f;
        [UIValue("clock-size")]
        public float ClockSize
        {
            get => _clockSize;
            set
            {
                _clockSize = value;
                NotifyPropertyChanged();
            }
        }

        public Color ClockColor
        {
            set
            {
                _clockTextObject.DefaultColor = value;
                _clockTextObject.color = value;
            }
        }

        public TMP_FontAsset Font
        {
            set => _clockTextObject.font = value;
        }

        public void Initialize()
        {

        }
    }
}