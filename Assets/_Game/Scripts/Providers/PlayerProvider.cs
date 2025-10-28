using System;
using _Game.Scripts.AssetLoader;
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
        
        private PlayerCharacter _player;
        private PlayerGun _playerGun;

        public event Action<PlayerCharacter> OnPlayerCreate;
        
        public async UniTask Init()
        {
            var prefab = await _levelAssetLoader.LoadWithoutSpawn<PlayerCharacter>("Player");
            
            _player = _playerFactory.Create(prefab);
            _playerGun = _player.GetComponent<PlayerGun>();
            
            OnPlayerCreate?.Invoke(_player);
        }

        public PlayerCharacter GetCharacter()
        {
            return _player;
        }

        public PlayerGun GetGun()
        { 
            return _playerGun;
        }
    }
}