using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.UI.Presenter;
using Code.Infrastructure.UI.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _container;
        private readonly Transform _uiRoot;
        private readonly List<IView> _views = new List<IView>();
        private readonly Dictionary<string, GameObject> _assetInstances = new Dictionary<string, GameObject>();

        public UIFactory(IAssetProvider assetProvider, DiContainer container)
        {
            _assetProvider = assetProvider;
            _container = container;
            _uiRoot = CreateUIRoot();
        }

        public async UniTask<(TView View, TPresenter Presenter)> CreatePresenterAsync<TView, TPresenter>(string assetKey, params object[] presenterArgs) where TView : Component, IView where TPresenter : BasePresenter<TView>
        {
            TView view = await CreateView<TView>(assetKey);

            if (view == null)
            {
                return (null, null);
            }

            TPresenter presenter = (TPresenter)_container.Instantiate(typeof(TPresenter), new object[] { view }.Concat(presenterArgs).ToArray());
            presenter.Initialize();

            return (view, presenter);
        }

        public async UniTask<TView> CreateView<TView>(string assetKey) where TView : Component, IView
        {
            if (_views.OfType<TView>().Any())
            {
                return _views.OfType<TView>().First();
            }

            var prefab = await _assetProvider.Load<GameObject>(assetKey);
            var instance = _container.InstantiatePrefab(prefab, _uiRoot);
            var view = instance.GetComponent<TView>();

            if (view == null)
            {
                Debug.LogError($"Component {typeof(TView).Name} not found on prefab {assetKey}");
                Object.Destroy(instance);
                return null;
            }

            _views.Add(view);
            _assetInstances[assetKey] = instance;

            return view;
        }

        public void CloseView<TView>() where TView : IView
        {
            var view = _views.OfType<TView>().FirstOrDefault();

            if (view == null)
            {
                return;
            }

            var instance = (view as Component)?.gameObject;

            if (instance != null)
            {
                Object.Destroy(instance);
                _views.Remove(view);

                var assetKey = _assetInstances.FirstOrDefault(x => x.Value == instance).Key;

                if (string.IsNullOrEmpty(assetKey) == false)
                {
                    _assetProvider.ReleaseAssetsByLabel(assetKey);
                    _assetInstances.Remove(assetKey);
                }
            }
        }

        public void CloseView(IView view)
        {
            var instance = (view as Component)?.gameObject;

            if (instance != null)
            {
                Object.Destroy(instance);
                _views.Remove(view);

                var assetKey = _assetInstances.FirstOrDefault(x => x.Value == instance).Key;

                if (string.IsNullOrEmpty(assetKey) == false)
                {
                    _assetProvider.ReleaseAssetsByLabel(assetKey);
                    _assetInstances.Remove(assetKey);
                }
            }
        }

        private Transform CreateUIRoot()
        {
            var root = new GameObject("UIRoot").transform;
            Object.DontDestroyOnLoad(root.gameObject);
            return root;
        }
    }
}