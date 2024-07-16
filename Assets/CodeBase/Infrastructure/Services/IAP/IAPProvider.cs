using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPProvider : IDetailedStoreListener
    {
        private const string IAPConfigsPath = "IAP/products";

        public event Action Initialized;

        private IExtensionProvider _extensions;
        private IStoreController _controller;
        private IAPService _iapService;

        public Dictionary<string, ProductConfig> Configs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }

        public bool IsInitialized => _controller != null && _extensions != null;

        public void Initialize(IAPService iapService)
        {
            _iapService = iapService;
            Configs = new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();
            Load();

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (ProductConfig productConfig in Configs.Values)
                builder.AddProduct(productConfig.Id, productConfig.ProductType);

            UnityPurchasing.Initialize(this, builder);
        }

        public void StartPurchase(string productId) =>
            _controller.InitiatePurchase(productId);

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            foreach (Product product in _controller.products.all)
                Products.Add(product.definition.id, product);

            Initialized?.Invoke();

            Debug.Log("UnityPurchasing initialization success");
        }

        public void OnInitializeFailed(InitializationFailureReason error) =>
            Debug.Log($"UnityPurchasing OnInitializeFailed {error}");

        public void OnInitializeFailed(InitializationFailureReason error, string message) =>
            Debug.Log($"UnityPurchasing OnInitializeFailed {error}, message {message}");

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"UnityPurchasing ProcessPurchase success {purchaseEvent.purchasedProduct.definition.id}");

            return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
            Debug.LogError($"Product {product.definition.id} purchase failed, PurchaseFailureReason {failureReason}, transaction id {product.transactionID}");

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) =>
            Debug.LogError($"Product {product.definition.id} purchase failed, PurchaseFailureDescription {failureDescription.reason}, transaction id {product.transactionID}");

        private void Load() =>
            Configs = Resources
                .Load<TextAsset>(IAPConfigsPath)
                .text
                .ToDeserialized<ProductConfigWrapper>()
                .Configs
                .ToDictionary(x => x.Id, x => x);
    }
}
