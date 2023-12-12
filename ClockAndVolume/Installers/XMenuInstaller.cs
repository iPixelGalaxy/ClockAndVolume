using SiraUtil;
using ClockAndVolume.UI;
using ClockAndVolume.Clock;
using ClockAndVolume.Volume;
using ClockAndVolume.UI.Credits;
using Installer = Zenject.Installer;
using Zenject;
using System;

namespace ClockAndVolume.Installers
{
    public class XMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BasicClock>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuVolumeManager>().AsSingle();

            Container.Bind<BasicClockView>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<SettingsView>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<CreditsInfoView>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<BasicClockViewPreview>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<XSettingsFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
        }
    }
}