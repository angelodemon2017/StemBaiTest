using DG.Tweening;
using Models;
using Services;
using Signals;
using System.Linq;
using UI;
using UnityEngine;
using Zenject;

namespace FSM
{
    public class StateStartlevel : StateWithWindowChange<MainGamePlayWindow>
    {
        public const int SLOTS = 7;

        private GamePlayerService _gamePlayerService;
        private DataLevel _dataLevel;
        private PropsServices propsServices;

        public StateStartlevel(
            SignalBus signalBus,
            MainGamePlayWindow windowOfState,
            DiContainer di) :
            base(signalBus, windowOfState, di)
        { }

        public override void Enter()
        {
            base.Enter();
            _gamePlayerService = _di.Resolve<GamePlayerService>();
            _dataLevel = _di.Resolve<DataLevel>();
            _dataLevel.GenerateDataFigures();
            propsServices = _di.Resolve<PropsServices>();

            SpawnFigures();
            _signalBus.Subscribe<ClickOnFigure>(ClickFigure);
        }

        private void SpawnFigures()
        {
            float spawnDelay = 0f;
            foreach (var df in _dataLevel.Figures)
            {
                DOVirtual.DelayedCall(spawnDelay, () =>
                {
                    var figure = GameObject.Instantiate(
                        propsServices.GetFigure(df),
                        _gamePlayerService.SpawnPoint.position,
                        Quaternion.identity,
                        _gamePlayerService.SpawnPoint);

                    figure.Init(
                        _signalBus,
                        propsServices,
                        df);

                    figure.transform.localScale = Vector3.zero;
                    figure.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
                });
                spawnDelay += 0.2f;
            }
        }

        private void ClickFigure(ClickOnFigure figure)
        {
            _dataLevel.SelectedFigures.Add(figure.dataFigure);
            _dataLevel.Figures.Remove(figure.dataFigure);
            
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
            CheckCombo();
        }

        private void CheckCombo()
        {

        }

        protected override void InitWindow()
        {
            base.InitWindow();

            for (int i = 0; i < SLOTS; i++)
            {
                var newSlot = GameObject.Instantiate(GetWindow.SlotPrefab, GetWindow.SlotsParent);
                GetWindow.Slots.Add(newSlot);
            }

            GetWindow.ButtonMix.onClick.AddListener(MixField);
        }

        private void MixField()
        {

        }

        public override void Exit()
        {
            base.Exit();

            GetWindow.ButtonMix.onClick.RemoveListener(MixField);
            _signalBus.Unsubscribe<ClickOnFigure>(ClickFigure);
        }
    }
}