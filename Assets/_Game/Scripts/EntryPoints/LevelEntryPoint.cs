using _Game.Scripts.AssetLoader;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class LevelEntryPoint : IInitializable
{
    private readonly LevelAssetLoader _levelAssetLoader;
    private readonly MultiplayerManager _multiplayerManager;
    
    [Inject]
    public LevelEntryPoint(LevelAssetLoader loader, MultiplayerManager multiplayerManager)
    {
        _levelAssetLoader = loader;
        _multiplayerManager = multiplayerManager;
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
        await _multiplayerManager.Init();
    }

    public void Init()
    {
    }
}