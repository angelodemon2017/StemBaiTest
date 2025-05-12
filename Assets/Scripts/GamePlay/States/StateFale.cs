using Signals;
using UI;
using Zenject;

namespace FSM
{
    public class StateFale : StateWithWindowChange<FailWindow>
    {
        public StateFale(
            SignalBus signalBus,
            FailWindow windowOfState,
            DiContainer di) :
            base(signalBus, windowOfState, di) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            GetWindow.ButtonRestart.onClick.RemoveListener(RestartLevel);
        }

        protected override void InitWindow()
        {
            base.InitWindow();

            GetWindow.ButtonRestart.onClick.AddListener(RestartLevel);
        }

        private void RestartLevel()
        {
            GetSignalBus.Fire(new RestartSignal());
        }
    }
}