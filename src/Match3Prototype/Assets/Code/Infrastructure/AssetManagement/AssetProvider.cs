using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Code.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider, IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public async Task InitializeAsync()
        {
            await Addressables.InitializeAsync().Task;
        }

        public async Task<TAsset> Load<TAsset>(string key) where TAsset : class
        {
            if (_completedCache.TryGetValue(key, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as TAsset;
            }

            AsyncOperationHandle<IList<IResourceLocation>> validateAddress = Addressables.LoadResourceLocationsAsync(key);
            await validateAddress.Task;

            if (validateAddress.Status != AsyncOperationStatus.Succeeded || validateAddress.Result.Count == 0)
            {
                Addressables.Release(validateAddress);
                return null;
            }

            AsyncOperationHandle<IList<TAsset>> handle = Addressables.LoadAssetsAsync<TAsset>(key);

            try
            {
                AddHandle(key, handle);
                await handle.Task;
                return handle.Result as TAsset;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading asset {key}: {e.Message}");
                return null;
            }
        }

        public async Task<TAsset> Instantiate<TAsset>(string address) where TAsset : class
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address);
            AddHandle(address, handle);

            await handle.Task;
            return handle.Result as TAsset;
        }

        public async Task<TAsset> Instantiate<TAsset>(string address, Vector3 at) where TAsset : class
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address, at, Quaternion.identity);
            AddHandle(address, handle);

            await handle.Task;
            return handle.Result as TAsset;
        }

        public async Task<TAsset> Instantiate<TAsset>(string address, Transform under) where TAsset : class
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address, under);
            AddHandle(address, handle);

            await handle.Task;
            return handle.Result as TAsset;
        }

        public async Task<TAsset> Instantiate<TAsset>(string address, Vector3 at, Transform under) where TAsset : class
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address, at, Quaternion.identity, under);
            AddHandle(address, handle);

            await handle.Task;
            return handle.Result as TAsset;
        }

        public async Task<SceneInstance> LoadScene(string sceneName)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName);
            AddHandle(sceneName, handle);
            return await handle.Task;
        }

        public async Task ReleaseAssetsByLabel(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);

            foreach (var assetKey in assetsList)
            {
                if (_handles.TryGetValue(assetKey, out var handles))
                {
                    foreach (var handle in handles.Where(handle => handle.IsValid()))
                    {
                        Addressables.Release(handle);
                    }

                    _handles.Remove(assetKey);
                }
            }
        }

        public void Dispose()
        {
            Cleanup();
        }

        private async Task<List<string>> GetAssetsListByLabel(string label, Type type = null)
        {
            var operationHandle = Addressables.LoadResourceLocationsAsync(label, type);
            var locations = await operationHandle.Task;
            var assetKeys = new List<string>(locations.Count);

            assetKeys.AddRange(locations.Select(location => location.PrimaryKey));
            Addressables.Release(operationHandle);

            return assetKeys;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle)
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new List<AsyncOperationHandle>();
                _handles[key] = handles;
            }

            handles.Add(handle);
        }

        private void Cleanup()
        {
            foreach (var resourceHandles in _handles.Values)
            {
                foreach (var handle in resourceHandles.Where(handle => handle.IsValid()))
                {
                    Addressables.Release(handle);
                }
            }

            _completedCache.Clear();
            _handles.Clear();
        }
    }
}