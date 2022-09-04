using Zenject;
using SiraUtil;
using ClockAndVolume.Clock;
using ClockAndVolume.Volume;
using System;

namespace ClockAndVolume.Installers
{
    public class XGameInstaller : Installer
    {
        private readonly ClockSettings _clockSettings;
        private readonly PlayerDataModel _playerDataModel;

        public XGameInstaller(ClockSettings clockSettings, PlayerDataModel playerDataModel)
        {
            _clockSettings = clockSettings;
            _playerDataModel = playerDataModel;
        }

        public override void InstallBindings()
        {
            var textAndHuds = !_playerDataModel.playerData.playerSpecificSettings.noTextsAndHuds;
            if (_clockSettings.Enabled && _clockSettings.ShowInGame && textAndHuds)
            {
                Container.Bind(typeof(BasicClockView), typeof(IInitializable)).To<BasicClockView>().FromNewComponentAsViewController().AsSingle();
                Container.BindInterfacesTo<GameClock>().AsSingle();
            }
            Container.BindInterfacesAndSelfTo<GameVolumeModifier>().AsSingle();
        }
    }
}