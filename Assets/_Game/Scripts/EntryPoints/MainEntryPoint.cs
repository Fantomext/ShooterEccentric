using _Game.Scripts.Helpers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _Game.Scripts.EntryPoints
{
    public class MainEntryPoint : IInitializable
    {
        private readonly SceneLoader _sceneLoader;
        
        [Inject]
        public MainEntryPoint(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async void Initialize()
        {
            await InitAsync();
            Init();
            
            //Пока нету меню
            _sceneLoader.LoadScene(1).Forget();

        }

        public async UniTask InitAsync()
        {
            
        }

        public void Init()
        {
            
        }
    }
}