using FSM;
using Models;
using Services;
using Signals;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private ConfigPropsLibrary _dialogsConfig;
    [SerializeField] private UIPrefabsConfig _uIPrefabsConfig;

    public override void InstallBindings()
    {
        InstallSignals();
        InstallConfigs();
        InstallServices();
        InstallStateMachine();
    }

    private void InstallConfigs()
    {
        Container.Bind<ConfigPropsLibrary>().FromScriptableObject(_dialogsConfig).AsSingle();
        Container.Bind<UIPrefabsConfig>().FromScriptableObject(_uIPrefabsConfig).AsSingle();
    }

    private void InstallServices()
    {
        Container.Bind<DataLevel>().FromInstance(new DataLevel(_dialogsConfig)).AsSingle();
        Container.Bind<PropsServices>().FromInstance(new PropsServices(_dialogsConfig)).AsSingle();
    }

    private void InstallStateMachine()
    {
        var gameplayStateMachine = new GamePlayerService(
            _uIPrefabsConfig.GameArena,
            Container.Resolve<DataLevel>());

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
        gameplayStateMachine.Register<StateStartlevel>(
            new StateStartlevel(
                Container.Resolve<SignalBus>(),
                _uIPrefabsConfig.MainGamePlayWindow,
                Container));
        gameplayStateMachine.Register<StateMainGameplay>(new StateMainGameplay());

        Container.Bind<GamePlayerService>().FromInstance(gameplayStateMachine).AsSingle();
    }/**/

/*    private void InstFSMByDS()
    {
        Container.Bind<StateWin>()
            .FromNew()
            .AsTransient()
            .WithArguments(_uIPrefabsConfig.WinWindow);

        Container.Bind<StateFale>()
                 .FromNew()
                 .AsTransient()
                 .WithArguments(_uIPrefabsConfig.FailWindow);

        Container.Bind<StateMainGameplay>()
                 .FromNew()
                 .AsTransient();

        Container.Bind<StateStartlevel>()
                 .FromNew()
                 .AsTransient();

        Container.Bind<GamePlayerService>()
                 .FromNew()
                 .AsSingle()
                 .OnInstantiated<GamePlayerService>((ctx, stateMachine) =>
                 {
                     stateMachine.Register<StateWin>(ctx.Container.Resolve<StateWin>());
                     stateMachine.Register<StateFale>(ctx.Container.Resolve<StateFale>());
                     stateMachine.Register<StateMainGameplay>(ctx.Container.Resolve<StateMainGameplay>());
                     stateMachine.Register<StateStartlevel>(ctx.Container.Resolve<StateStartlevel>());
                 });
    }/**/

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<NextLevel>();
        Container.DeclareSignal<StartLevel>();
        Container.DeclareSignal<ClickOnFigure>();
    }
}