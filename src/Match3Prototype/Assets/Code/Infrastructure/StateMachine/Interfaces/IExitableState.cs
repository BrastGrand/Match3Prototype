using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.Interfaces
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}