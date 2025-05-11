using UI;
using UnityEngine;
using Zenject;

namespace FSM
{
    public class StateWithWindowChange<T> : IState where T : MainWindow
    {
        protected readonly SignalBus _signalBus;
        private readonly T _windowPrefabLink;
        protected readonly DiContainer _di;

        private T _windowState;

        protected SignalBus GetSignalBus => _signalBus;
        protected T GetWindow => _windowState;

        public StateWithWindowChange(
            SignalBus signalBus,
            T windowState,
            DiContainer di)
        {
            _signalBus = signalBus;
            _windowPrefabLink = windowState;
            _di = di;
        }

        protected virtual void InitWindow() { }

        public virtual void Enter()
        {
            _windowState = GameObject.Instantiate(_windowPrefabLink);
            _windowState.Show();
            InitWindow();
        }

        public virtual void Exit()
        {
            _windowState.Hide();
        }
    }
}