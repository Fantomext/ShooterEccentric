using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Game.Scripts.EntryPoints
{
    public class MainEntryPoint : IInitializable
    {
        public MainEntryPoint()
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
}