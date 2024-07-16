using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Inputs;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, EnterLoadLevel);
        }

        public void Exit()
        {

        }

        private void EnterLoadLevel() => _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            RegisterStaticData();
            RegisterAdsService();

            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IRandomService>(new RandomService());
            RegisterAssetProvider();
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            RegisterIAPService(new IAPProvider(), _services.Single<IPersistentProgressService>());

            _services.RegisterSingle<IUIFactory>(new UIFactory(
                _services.Single<IAssets>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IAdsService>(),
                _services.Single<IIAPService>()
            ));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssets>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IRandomService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IWindowService>()
            ));

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
        }

        private void RegisterAssetProvider()
        {
            var assetProvider = new AssetProvider();
            assetProvider.Initialize();
            _services.RegisterSingle<IAssets>(assetProvider);
        }

        private void RegisterAdsService()
        {
            var adService = new AdsService();
            adService.Initialize();
            _services.RegisterSingle<IAdsService>(adService);
        }

        private void RegisterIAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            var iapService = new IAPService(iapProvider, progressService);
            iapService.Initialize();
            _services.RegisterSingle<IIAPService>(iapService);
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private static IInputService InputService()
        {
            if(Application.isEditor)
            {
                return new StandaloneInputService();
            }
            else
            {
                return new MobileInputService();
            }
        }
    }
}