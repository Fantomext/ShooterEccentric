using System;
using _Game.Scripts.AssetLoader;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class LevelEntryPoint : IInitializable
{
    
    [Inject]
    public LevelEntryPoint(LevelAssetLoader loader)
    {
        
    }
    
    public async void Initialize()
    {
        await InitAsync();
        Init();
            
    }

    public async UniTask InitAsync()
    {
        
    }

    public void Init()
    {
            
    }
}