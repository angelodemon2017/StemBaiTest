using FSM;
using Models;
using Services;
using Signals;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private bool IsEasy;

    [SerializeField] private ConfigPropsLibrary _hardConfig;
    [SerializeField] private ConfigPropsLibrary _easyConfig;
    [SerializeField] private UIPrefabsConfig _uIPrefabsConfig;

    private ConfigPropsLibrary GetConfig =>
        IsEasy ?
        _easyConfig :
        _hardConfig;

    public override void InstallBindings()
    {
        InstallSignals();
        InstallConfigs();
        InstallServices();
        InstallStateMachine();
    }

    private void InstallConfigs()
    {
        Container.Bind<ConfigPropsLibrary>()
            .FromScriptableObject(GetConfig).AsSingle();

        Container.Bind<UIPrefabsConfig>()
            .FromScriptableObject(_uIPrefabsConfig).AsSingle();
    }

    private void InstallServices()
    {
        Container.Bind<DataLevel>().FromInstance(new DataLevel(GetConfig)).AsSingle();
        Container.Bind<PropsServices>().FromInstance(new PropsServices(GetConfig)).AsSingle();
    }

    private void InstallStateMachine()
    {
        var gameplayStateMachine = new GamePlayerService(
            _uIPrefabsConfig.GameArena,
            Container.Resolve<DataLevel>(),
            Container.Resolve<SignalBus>());

        gameplayStateMachine.Register<StateWin>(
            new StateWin(
                Container.Resolve<SignalBus>(),
                _uIPrefabsConfig.WinWindow,
                Container));
        gameplayStateMachine.Register<StateFale>(
            new StateFale(
                Container.Resolve<SignalBus>(),
                _uIPrefabsConfig.FailWindow,
                Container));
        gameplayStateMachine.Register<StateMainGameplay>(
            new StateMainGameplay(
                Container.Resolve<SignalBus>(),
                _uIPrefabsConfig.MainGamePlayWindow,
                Container));

        Container.Bind<GamePlayerService>().FromInstance(gameplayStateMachine).AsSingle();
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<NextLevelSignal>();
        Container.DeclareSignal<AddScoreSignal>();
        Container.DeclareSignal<RestartSignal>();
        Container.DeclareSignal<ClickOnFigureSignal>();
    }
}