using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Factory;
using VContainer;
using VContainer.Unity;

public class LevelLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterEntryPoint<LevelEntryPoint>();

        builder.Register<LevelAssetLoader>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<MultiplayerManager>();

        builder.Register<GameObjectFactory<Character>>(Lifetime.Singleton);
        builder.Register<GameObjectFactory<EnemyController>>(Lifetime.Singleton);
    }
}