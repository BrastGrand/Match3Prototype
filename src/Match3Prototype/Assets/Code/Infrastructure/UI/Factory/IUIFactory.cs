using Code.Infrastructure.UI.Presenter;
using Code.Infrastructure.UI.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.UI.Factory
{
    public interface IUIFactory
    {
        UniTask<TView> CreateView<TView>(string assetKey) where TView : Component, IView;

        UniTask<(TView View, TPresenter Presenter)> CreatePresenterAsync<TView, TPresenter>(string assetKey, params object[] presenterArgs)
            where TView : Component, IView
            where TPresenter : BasePresenter<TView>;

        void CloseView<TView>() where TView : IView;

        void CloseView(IView view);
    }
}