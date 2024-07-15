using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPProvider : IStoreListener
    {
        private const string IAPConfigsPath = "IAP/products";

        private List<ProductConfig> _configs;

        public void Initialize()
        {
            Load();

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (ProductConfig productConfig in _configs)
                builder.AddProduct(productConfig.Id, productConfig.Type);

            UnityPurchasing.Initialize(this, builder);
        }

        private void Load()
        {
            _configs = Resources.Load<TextAsset>(IAPConfigsPath).text.ToDeserialized<ProductConfigWrapper>().Configs;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            throw new System.NotImplementedException();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            throw new System.NotImplementedException();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            throw new System.NotImplementedException();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            throw new System.NotImplementedException();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            throw new System.NotImplementedException();
        }
    }
}
