using _Game.Scripts.Providers;
using VContainer;

namespace _Game.Scripts
{
    public class HealthUIPlayer : HealthUI
    {
        private PlayerProvider _playerProvider;
        
        private void OnEnable()
        {
            _health.OnChangeHealth += UpdateHealth;
        }

        private void OnDisable()
        {
            _health.OnChangeHealth -= UpdateHealth;
        }
        
        [Inject]
        public void Init(PlayerProvider provider)
        {
            _playerProvider = provider;

           _health = _playerProvider.GetHealth();

        }
    }
}