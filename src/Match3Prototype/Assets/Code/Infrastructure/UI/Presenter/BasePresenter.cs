using Code.Infrastructure.UI.View;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.UI.Presenter
{
    public abstract class BasePresenter<TView> : IPresenter<TView> where TView : IView
    {
        private bool _isInitialized = false;
        public TView View { get; protected set; }

        protected BasePresenter(TView view)
        {
            View = view;
        }

        public virtual void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            OnInitialize();
            _isInitialized = true;
        }

        public virtual async UniTask Show()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            await OnShow();
            await View.Show();
        }

        public virtual async UniTask Hide()
        {
            await OnHide();
            await View.Hide();
        }

        public virtual void Cleanup()
        {
            OnCleanup();
            _isInitialized = false;
        }

        protected virtual void OnInitialize() { }

        protected virtual UniTask OnShow() => UniTask.CompletedTask;

        protected virtual UniTask OnHide() => UniTask.CompletedTask;

        protected virtual void OnCleanup() { }
    }
}