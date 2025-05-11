using Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class GamePlayerService : IStateMachine
    {
        private GameArena _gameArenaPrefab;
        private GameArena _gameArena;
        private DataLevel _dataLevel;

        private readonly Dictionary<Type, IState> _states = new();
        private IState _gameState;

        public Transform SpawnPoint => _gameArena.GetSpawnPoint;
        public IState CurrentState => _gameState;

        public GamePlayerService(GameArena gameArenaPrefab, DataLevel dataLevel)
        {
            _gameArenaPrefab = gameArenaPrefab;
            _dataLevel = dataLevel;
        }

        public void StartGame()
        {
            _gameArena = GameObject.Instantiate(_gameArenaPrefab);
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