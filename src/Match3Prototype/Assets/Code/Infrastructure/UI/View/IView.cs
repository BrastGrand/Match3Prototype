using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.UI.View
{
    public interface IView
    {
        UniTask Show();

        UniTask Hide();

        void Initialize();

        void Cleanup();
    }
}