using System;
using Code.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Loading
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IAssetProvider _assetProvider;

        public SceneLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask LoadScene(string nextScene)
        {
            try
            {
                await _assetProvider.LoadScene(nextScene);
                Debug.Log($"Completed loading scene: {nextScene}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading scene {nextScene} : {e.Message}");
            }
        }
    }
}