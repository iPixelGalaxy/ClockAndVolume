using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace ClockAndVolume.UI.Credits
{
    [ViewDefinition("ClockAndVolume.Views.credits-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\..\Views\credits-view.bsml")]
    public class CreditsInfoView : BSMLAutomaticViewController
    {
    }
}