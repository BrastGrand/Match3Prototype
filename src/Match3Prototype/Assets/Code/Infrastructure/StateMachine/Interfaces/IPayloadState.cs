using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.Interfaces
{
    public interface IPayloadState<in T> : IExitableState
    {
        UniTask Enter(T payload);
    }
}