using _Game.Scripts;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Gun;
using _Game.Scripts.Multiplayer;
using _Game.Scripts.Providers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class LevelEntryPoint : IInitializable
{
    private readonly LevelAssetLoader _levelAssetLoader;
    private readonly MultiplayerManager _multiplayerManager;
    private readonly InputSystem _inputSystem;
    private readonly BulletPool _bulletPool;
    private readonly PlayerProvider _playerProvider;
    private readonly ServerPlayerConnector _serverPlayerConnector;
    
    [Inject]
    public LevelEntryPoint(LevelAssetLoader loader, 
        MultiplayerManager multiplayerManager, 
        InputSystem inputSystem, BulletPool bulletPool, 
        PlayerProvider provider,
        ServerPlayerConnector playerConnector)
    {
        _levelAssetLoader = loader;
        _multiplayerManager = multiplayerManager;
        _inputSystem = inputSystem;
        _bulletPool = bulletPool;
        _playerProvider = provider;
        _serverPlayerConnector = playerConnector;
    }
    
    public async void Initialize()
    {
        await InitAsync();
        Init();
        //Игра готова к старту
            
    }

    public async UniTask InitAsync()
    {
        //Добавить загрузку всех ассетов, чтобы не каждый класс сам просил её.
        await _bulletPool.Init();
        await _playerProvider.Init();
        await _multiplayerManager.Init();
    }

    public void Init()
    {
        _serverPlayerConnector.Init();
        _inputSystem.Initialize();
    }
}