using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItemsContainer : MonoBehaviour
    {
        private const string ShopItemPath = "ShopItem";

        private readonly List<GameObject> _shopItems = new List<GameObject>();

        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;

        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssets _assets;

        public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssets assets)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assets = assets;
        }

        public void Initialize() =>
            RefreshAvailableItems();

        public void Subscribe()
        {
            _iapService.Initialized += RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
        }

        public void Cleanup()
        {
            _iapService.Initialized -= RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateUnavailableObjects();

            if (!_iapService.IsInitialized)
                return;

            ClearShopItems();

            await FillShopItems();
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems)
                Destroy(shopItem);
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject shopItemObject = await _assets.Instantiate(ShopItemPath, Parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();

                shopItem.Construct(productDescription, _iapService, _assets);

                shopItem.Initialize();

                _shopItems.Add(shopItemObject);
            }
        }

        private void UpdateUnavailableObjects()
        {
            foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
                shopUnavailableObject.SetActive(!_iapService.IsInitialized);
        }
    }
}
