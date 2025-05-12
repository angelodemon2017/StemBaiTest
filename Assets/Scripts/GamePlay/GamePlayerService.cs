using Models;
using Signals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FSM
{
    public class GamePlayerService : IStateMachine
    {
        private SignalBus _signalBus;
        private GameArena _gameArenaPrefab;
        private GameArena _gameArena;
        private DataLevel _dataLevel;

        private readonly Dictionary<Type, IState> _states = new();
        private IState _gameState;

        public Transform SpawnPoint => _gameArena.GetSpawnPoint;
        public IState CurrentState => _gameState;

        public GamePlayerService(GameArena gameArenaPrefab, DataLevel dataLevel, SignalBus signalBus)
        {
            _gameArenaPrefab = gameArenaPrefab;
            _dataLevel = dataLevel;
            _signalBus = signalBus;

            _signalBus.Subscribe<RestartSignal>(Restart);
            _signalBus.Subscribe<NextLevelSignal>(NextLevel);
            _signalBus.Subscribe<AddScoreSignal>(AddScore);
        }

        public void StartGame()
        {
            _gameArena = GameObject.Instantiate(_gameArenaPrefab);
        }

        private void Restart()
        {
            _dataLevel.Score = 0;
            Enter<StateMainGameplay>();
        }

        private void NextLevel()
        {
            _dataLevel.Score = 0;
            Enter<StateMainGameplay>();
        }

        private void AddScore()
        {
            _dataLevel.Score++;
        }

        public void Register<TState>(TState state) where TState : IState
        {
            var type = typeof(TState);
            _states[type] = state;
        }

        public void Enter<TState>() where TState : IState
        {
            var type = typeof(TState);
            if (!_states.TryGetValue(type, out var newState))
                throw new KeyNotFoundException($"State {type.Name} not registered!");

            _gameState?.Exit();
            _gameState = newState;
            _gameState.Enter();
        }

        public void ExitGame()
        {
            if (_gameArena)
            {
                GameObject.Destroy(_gameArena);
            }
        }

        public bool HasState<TState>() where TState : IState
        {
            return _states.ContainsKey(typeof(TState));
        }
    }
}