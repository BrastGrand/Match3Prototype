using Code.Infrastructure.UI.View;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.UI.Presenter
{
    public interface IPresenter<TView> where TView : IView
    {
        TView View { get; }

        void Initialize();

        UniTask Show();

        UniTask Hide();

        void Cleanup();
    }
}