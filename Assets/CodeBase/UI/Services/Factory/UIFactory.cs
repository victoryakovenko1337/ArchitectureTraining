using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";
        private readonly IStaticDataService _staticData;
        private readonly IAssets _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adService;
        private readonly IIAPService _iapService;

        private Transform _uiRoot;

        public UIFactory(
            IAssets assets,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            IAdsService adService,
            IIAPService iapService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adService = adService;
            _iapService = iapService;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            window.Construct(_adService, _progressService, _iapService, _assets);
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }
    }
}
