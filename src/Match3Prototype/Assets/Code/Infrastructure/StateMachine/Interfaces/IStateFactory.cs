namespace Code.Infrastructure.StateMachine.Interfaces
{
    public interface IStateFactory
    {
        T GetState<T>() where T : class, IExitableState;
    }
}