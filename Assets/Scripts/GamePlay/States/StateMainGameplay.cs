using DG.Tweening;
using Extensions;
using Models;
using Services;
using Signals;
using System.Linq;
using UI;
using UnityEngine;
using Zenject;

namespace FSM
{
    public class StateMainGameplay : StateWithWindowChange<MainGamePlayWindow>
    {
        public const int SLOTS = 7;

        private GamePlayerService _gamePlayerService;
        private DataLevel _dataLevel;
        private PropsServices _propsServices;
        private Sequence _spawnSequence;

        public StateMainGameplay(
            SignalBus signalBus,
            MainGamePlayWindow windowOfState,
            DiContainer di) :
            base(signalBus, windowOfState, di)
        { }

        public override void Enter()
        {
            _propsServices = _di.Resolve<PropsServices>();
            base.Enter();
            _gamePlayerService = _di.Resolve<GamePlayerService>();
            _dataLevel = _di.Resolve<DataLevel>();
            _dataLevel.GenerateDataFigures();

            SpawnFigures();
            _signalBus.Subscribe<ClickOnFigureSignal>(ClickFigure);
        }

        private void SpawnFigures()
        {
            _spawnSequence = DOTween.Sequence();
            _gamePlayerService.SpawnPoint.DestroyChildrens();

            GetWindow.ButtonMix.interactable = false;
            int totalFigures = _dataLevel.Figures.Count;
            foreach (var df in _dataLevel.Figures)
            {
                _spawnSequence.AppendCallback(() =>
                {
                    var figure = GameObject.Instantiate(
                        _propsServices.GetFigure(df),
                        _gamePlayerService.SpawnPoint.position,
                        Quaternion.identity,
                        _gamePlayerService.SpawnPoint);

                    figure.Init(_signalBus, _propsServices, df);

                    figure.transform.localScale = Vector3.zero;
                    figure.transform.DOScale(Vector3.one * _propsServices.GetScaleFactor, 0.3f)
                        .SetEase(Ease.OutBack);
                });

                _spawnSequence.AppendInterval(0.1f);
            }

            _spawnSequence.OnComplete(() =>
            {
                GetWindow.ButtonMix.interactable = true;
            });
        }

        public void StopSpawn()
        {
            _spawnSequence?.Kill();
            GetWindow.ButtonMix.interactable = true;
        }

        private void ClickFigure(ClickOnFigureSignal figure)
        {
            _dataLevel.SelectedFigures.Add(figure.dataFigure);
            _dataLevel.Figures.Remove(figure.dataFigure);

            CheckCombo();
            UpdateSlots();
            CheckGameState();
        }

        private void UpdateSlots()
        {
            for (int i = 0; i < SLOTS; i++)
            {
                if (i < _dataLevel.SelectedFigures.Count)
                {
                    GetWindow.Slots[i].UpdateView(_dataLevel.SelectedFigures[i]);
                }
                else
                {
                    GetWindow.Slots[i].Clean();
                }
            }
        }

        private void CheckCombo()
        {
            var figureGroup = _dataLevel.SelectedFigures
                .GroupBy(f => f.IdFigureProp)
                .FirstOrDefault(g => g.Count() >= 3);
            if (figureGroup != null)
            {
                RemoveMatchedFigures(figureGroup.Key, f => f.IdFigureProp);
                return;
            }
            var colorGroup = _dataLevel.SelectedFigures
                .GroupBy(f => f.IdColorProp)
                .FirstOrDefault(g => g.Count() >= 3);

            if (colorGroup != null)
            {
                RemoveMatchedFigures(colorGroup.Key, f => f.IdColorProp);
                return;
            }
            
            var imageGroup = _dataLevel.SelectedFigures
                .GroupBy(f => f.IdImageProp)
                .FirstOrDefault(g => g.Count() >= 3);

            if (imageGroup != null)
            {
                RemoveMatchedFigures(imageGroup.Key, f => f.IdImageProp);
            }
        }

        private void CheckGameState()
        {
            if (_dataLevel.SelectedFigures.Count >= SLOTS ||
                (_dataLevel.Figures.Count == 0 && _dataLevel.SelectedFigures.Count > 0))
            {
                _gamePlayerService.Enter<StateFale>();
            }
            if (_dataLevel.SelectedFigures.Count == 0 &&
                _dataLevel.Figures.Count == 0)
            {
                _gamePlayerService.Enter<StateWin>();
            }
        }

        private void RemoveMatchedFigures<TKey>(TKey matchedId, System.Func<DataFigure, TKey> idSelector)
        {
            var matches = _dataLevel.SelectedFigures.Where(f => idSelector(f).Equals(matchedId)).ToList();

            int removeCount = System.Math.Min(3, matches.Count);

            for (int i = 0; i < removeCount; i++)
            {
                _dataLevel.SelectedFigures.Remove(matches[i]);
            }

            _signalBus.Fire(new AddScoreSignal());
        }

        protected override void InitWindow()
        {
            base.InitWindow();

            for (int i = 0; i < SLOTS; i++)
            {
                var newSlot = GameObject.Instantiate(GetWindow.SlotPrefab, GetWindow.SlotsParent);
                newSlot.Init(_propsServices);
                GetWindow.Slots.Add(newSlot);
            }

            GetWindow.ButtonMix.onClick.AddListener(MixField);
        }

        private void MixField()
        {
            _dataLevel.Figures.Shuffle();
            SpawnFigures();
        }

        public override void Exit()
        {
            base.Exit();

            StopSpawn();
            GetWindow.ButtonMix.onClick.RemoveListener(MixField);
            _signalBus.Unsubscribe<ClickOnFigureSignal>(ClickFigure);
        }
    }
}