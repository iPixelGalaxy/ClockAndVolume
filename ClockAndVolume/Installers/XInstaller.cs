using Zenject;
using ClockAndVolume.Clock;
using Version = Hive.Versioning.Version;

namespace ClockAndVolume.Installers
{
    public class XInstaller : Installer<Config, Version, XInstaller>
    {
        private readonly Config _config;
        private readonly Version _version;

        public XInstaller(Config config, Version version)
        {
            _config = config;
            _version = version;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();
            Container.BindInstance(_config.Clock).AsSingle();
            Container.BindInstance(_config.Volume).AsSingle();
            Container.Bind(typeof(IClockController), typeof(ITickable), typeof(ClockController)).To<ClockController>().AsSingle();
            Container.Bind<XLoader>().AsSingle().Lazy();
            Container.Bind<Version>().WithId("ClockAndVolume.Version").FromInstance(_version).AsCached();
        }
    }
}