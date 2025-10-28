using _Game.Scripts.EntryPoints;
using _Game.Scripts.Helpers;
using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterEntryPoint<MainEntryPoint>(Lifetime.Singleton);

        builder.Register<SceneLoader>(Lifetime.Singleton);
    }
}