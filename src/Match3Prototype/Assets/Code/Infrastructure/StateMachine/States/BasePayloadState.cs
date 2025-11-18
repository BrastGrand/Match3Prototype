using Code.Infrastructure.StateMachine.Interfaces;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class BasePayloadState<TPayload> : IPayloadState<TPayload>
    {
        public virtual UniTask Enter(TPayload payload)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}