using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Factory;
using _Game.Scripts.Gun;
using _Game.Scripts.Multiplayer;
using _Game.Scripts.Providers;
using VContainer;
using VContainer.Unity;

public class LevelLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterEntryPoint<LevelEntryPoint>();

        builder.Register<LevelAssetLoader>(Lifetime.Scoped);
        builder.Register<PlayerProvider>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<MultiplayerManager>();
        builder.Register<ServerConnector>(Lifetime.Singleton);

        builder.Register<GameObjectFactory<PlayerCharacter>>(Lifetime.Singleton);
        builder.Register<GameObjectFactory<EnemyController>>(Lifetime.Singleton);
        builder.Register<GameObjectPool<Bullet>>(Lifetime.Singleton);

        builder.Register<InputSystem>(Lifetime.Singleton);
        builder.Register<BulletPool>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<PlayerCamera>();

    }
}