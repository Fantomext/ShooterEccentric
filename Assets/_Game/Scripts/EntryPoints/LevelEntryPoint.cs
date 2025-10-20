using System;
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
            
    }

    public async UniTask InitAsync()
    {
        //Загрузить все ассеты через assetloader
    }

    public void Init()
    {
            _multiplayerManager.Init();
    }
}