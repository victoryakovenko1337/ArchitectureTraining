using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagment
{
    public class AssetProvider : IAssets
    {
        private Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);

            handle.Completed += h =>
            {
                _completedCache[assetReference.AssetGUID] = h;
            };

            AddHandle(assetReference.AssetGUID, handle);

            return await handle.Task;
        }

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
            foreach (AsyncOperationHandle handle in resourceHandles)
                Addressables.Release(handle);
        }

        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }
    }
}