using Code.Infrastructure.StateMachine.Interfaces;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class BaseState : IState
    {
        public virtual UniTask Enter()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}