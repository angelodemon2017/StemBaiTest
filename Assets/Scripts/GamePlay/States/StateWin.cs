using Signals;
using UI;
using UnityEngine;
using Zenject;

namespace FSM
{
    public class StateWin : StateWithWindowChange<WinWindow>
    {
        public StateWin(
            SignalBus signalBus,
            WinWindow windowOfState,
            DiContainer di) :
            base(signalBus, windowOfState, di) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            GetWindow.ButtonNextLevel.onClick.RemoveListener(LoadNextLevel);
        }

        protected override void InitWindow()
        {
            base.InitWindow();

            GetWindow.ButtonNextLevel.onClick.AddListener(LoadNextLevel);
        }

        private void LoadNextLevel()
        {
            Debug.Log($"LoadNextLevel");
            GetSignalBus.Fire(new NextLevel());
        }
    }
}