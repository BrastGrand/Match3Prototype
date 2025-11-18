using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.Interfaces
{
    public interface IStateMachine
    {
        UniTask Enter<TState>() where TState : class, IState;
        UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
    }
}