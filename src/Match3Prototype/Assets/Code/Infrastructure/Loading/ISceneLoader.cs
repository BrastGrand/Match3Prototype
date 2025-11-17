using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Loading
{
    public interface ISceneLoader
    {
        UniTask LoadScene(string nextScene);
    }
}