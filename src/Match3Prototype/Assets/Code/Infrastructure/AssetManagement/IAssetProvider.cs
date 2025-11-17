using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Code.Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        Task InitializeAsync();
        Task<TAsset> Load<TAsset>(string key) where TAsset : class;
        Task<TAsset> Instantiate<TAsset>(string address) where TAsset : class;
        Task<TAsset> Instantiate<TAsset>(string address, Vector3 at) where TAsset : class;
        Task<TAsset> Instantiate<TAsset>(string address, Transform under) where TAsset : class;
        Task<TAsset> Instantiate<TAsset>(string address, Vector3 at, Transform under) where TAsset : class;
        Task<SceneInstance> LoadScene(string sceneName);
    }
}
