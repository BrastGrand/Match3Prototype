using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.Interfaces
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}