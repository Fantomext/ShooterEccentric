using System;
using _Game.Scripts.AssetLoader;
using _Game.Scripts.Configs;
using _Game.Scripts.Factory;
using _Game.Scripts.Gun;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Game.Scripts.Providers
{
    public class PlayerProvider
    {
        [Inject] private GameObjectFactory<PlayerCharacter> _playerFactory;
        [Inject] private LevelAssetLoader _levelAssetLoader;
        [Inject] private PlayerConfig _playerConfig;
        
        private PlayerCharacter _player;
        private PlayerGun _playerGun;
        private Health _health;

        public event Action<PlayerCharacter> OnPlayerCreate;
        
        public async UniTask Init()
        {
            var prefab = await _levelAssetLoader.LoadWithoutSpawn<PlayerCharacter>("Player");
            
            _player = _playerFactory.Create(prefab);
            _playerGun = _player.GetComponent<PlayerGun>();
            _health = _player.GetComponent<Health>();
            
            _player.Speed = _playerConfig.Speed;
            _health.SetMax(_playerConfig.MaxHealth);
            
            OnPlayerCreate?.Invoke(_player);
        }

        public void SetSessionID(string sessionID)
        {
            _player.SessionID = sessionID;
            _playerGun.SetID(sessionID);
        }

        public PlayerCharacter GetCharacter()
        {
            return _player;
        }

        public PlayerGun GetGun()
        { 
            return _playerGun;
        }

        public Health GetHealth()
        {
            return _health;
        }
    }
}