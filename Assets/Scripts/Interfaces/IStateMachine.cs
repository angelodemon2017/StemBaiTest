namespace FSM
{
    public interface IStateMachine
    {
        IState CurrentState { get; }
        
        void Register<TState>(TState state) where TState : IState;

        void Enter<TState>() where TState : IState;

        bool HasState<TState>() where TState : IState;
    }
}
