using System;
using System.Collections.Generic;
using _Game.Scripts.Gun;
using _Game.Scripts.Providers;
using Colyseus.Schema;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Multiplayer
{
    public class ServerPlayerConnector : IDisposable
    {
        private MultiplayerManager _multiplayerManager;
        private PlayerProvider _playerProvider;
        private EnemyPool _enemyPool;
        
        private Player _player;
        
        private PlayerCharacter _character;
        private PlayerGun _playerGun;
        private Health _health;

        private const string ChangePosition = "move";
        private const string Shoot = "shoot";
        private const string Crouch = "sit";

        private bool _playerIsCrouched;

        public event Action<EnemyCharacter> PlayerDie;
        
        [Inject]
        public ServerPlayerConnector(MultiplayerManager multiplayerManager, PlayerProvider playerProvider, EnemyPool pool)
        {
            _multiplayerManager = multiplayerManager;
            _playerProvider = playerProvider;
            _enemyPool = pool;
        }

        public void Init()
        {
            _character = _playerProvider.GetCharacter();
            _playerGun = _playerProvider.GetGun();
            _health = _playerProvider.GetHealth();
            
            _character.OnMove += SendMessage;
            _playerGun.OnShootData += SendShoot;
            _character.OnCrouch += CharacterCrouch;
            _multiplayerManager.OnPlayerCreate += PlayerCreated;
            _multiplayerManager.OnPlayerRestart += Restart;
        }

        private void PlayerCreated(Player player)
        {
            _player = player;
            
            _player.OnChange += OnChange;
        }

        private void OnChange(List<DataChange> changes)
        {
            foreach (var change in changes)
            {
                switch (change.Field)
                {
                    case "curHP":
                        _health.SetCurrentHealth((sbyte) change.Value);
                        break;
                }
            }
            
        }

        private void SendShoot(ShootInfo shootInfo)
        {
            shootInfo.key = _multiplayerManager.ReturnSessionId();
            string jsonShoot = JsonUtility.ToJson(shootInfo);
            
            _multiplayerManager.SendMessage(Shoot, jsonShoot);
        }

        private void CharacterCrouch(bool isSit)
        {
            SitInfo sitInfo = new SitInfo();
            sitInfo.key = _multiplayerManager.ReturnSessionId();
            sitInfo.sit = isSit;
            
            string sitJson = JsonUtility.ToJson(sitInfo);
            _multiplayerManager.SendMessage(Crouch, sitJson);
        }
        
        private void SendMessage(Vector3 position, Vector3 velocity, Vector2 rotation)
        {
            Dictionary<string, object> data
                = new Dictionary<string, object>()
                {
                    { "pX", position.x},
                    { "pY", position.y},
                    { "pZ", position.z},
                    
                    { "vX", velocity.x},
                    { "vY", velocity.y},
                    { "vZ", velocity.z},
                    
                    {"rX", rotation.x},
                    {"rY", rotation.y},
                };
            
            _multiplayerManager.SendMessage(ChangePosition, data);
        }

        private void Restart(string jsonData)
        {
            var data = JsonUtility.FromJson<RestartInfo>(jsonData);
            var enemy = _enemyPool.GetEnemy(data.killer);

            _character.Restart(data, enemy);
        }

        public void Dispose()
        {
            _character.OnMove -= SendMessage;
            _playerGun.OnShootData -= SendShoot;
            _character.OnCrouch -= CharacterCrouch;
        }
    }
}