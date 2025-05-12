using FSM;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings() { }

    public override void Start()
    {
        base.Start();
        var gps = Container.Resolve<GamePlayerService>();
        gps.StartGame();
        gps.Enter<StateMainGameplay>();
    }
}