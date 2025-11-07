using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Configs;
using _Game.Scripts.Factory;
using _Game.Scripts.Gun;
using _Game.Scripts.Multiplayer;
using _Game.Scripts.Providers;
using _Game.Scripts.Spawners;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelLifeTimeScope : LifetimeScope
{
    [SerializeField] private PlayerConfig _playerConfig;
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterEntryPoint<LevelEntryPoint>();

        builder.Register<LevelAssetLoader>(Lifetime.Scoped);
        builder.Register<PlayerProvider>(Lifetime.Singleton);
        builder.Register<UIProvider>(Lifetime.Singleton);
        builder.Register<EnemyPool>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<MultiplayerManager>();
        builder.Register<ServerPlayerConnector>(Lifetime.Singleton);
        builder.Register<ServerEnemyConnector>(Lifetime.Singleton);

        builder.Register<GameObjectFactory<PlayerCharacter>>(Lifetime.Singleton);
        builder.Register<GameObjectFactory<EnemyController>>(Lifetime.Singleton);
        builder.Register<GameObjectPool<Bullet>>(Lifetime.Singleton);
        
        builder.Register<EnemySpawner>(Lifetime.Singleton);

        builder.Register<InputSystem>(Lifetime.Singleton);
        builder.Register<BulletPool>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<PlayerCamera>();

        builder.RegisterInstance(_playerConfig);

    }
}