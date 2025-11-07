using _Game.Scripts.AssetLoader;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Game.Scripts.Providers
{
    public class UIProvider
    {
        private readonly LevelAssetLoader _levelAssetLoader;  
        private HealthUIPlayer _playerHealth;

        [Inject]
        public UIProvider(LevelAssetLoader levelAssetLoader)
        {
            _levelAssetLoader = levelAssetLoader;
        }
        
        public async UniTask Init()
        {
            _playerHealth = await _levelAssetLoader.Load<HealthUIPlayer>("playerHP");
        }

        public HealthUIPlayer GetPlayerHealth()
        {
            return _playerHealth;
        }
    }
}