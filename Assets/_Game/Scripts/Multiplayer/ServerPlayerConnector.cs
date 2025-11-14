using System;
using System.Collections.Generic;
using _Game.Scripts.Gun;
using _Game.Scripts.Gun.StateMachine.Enum;
using _Game.Scripts.Info;
using _Game.Scripts.Providers;
using Colyseus.Schema;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Multiplayer
{
    public class ServerPlayerConnector : IDisposable
    {
        private readonly ScoreManager _scoreManager;
        private readonly MultiplayerManager _multiplayerManager;
        private readonly PlayerProvider _playerProvider;
        private readonly EnemyPool _enemyPool;
        private readonly SpawnPointManager _spawnPointManager;
        
        private Player _player;
        
        private PlayerCharacter _character;
        private PlayerGun _playerGun;
        private Health _health;

        private const string ChangePosition = "move";
        private const string Shoot = "shoot";
        private const string Crouch = "sit";
        private const string Win = "win";
        private const string SwapWeapon = "swap";

        private bool _playerIsCrouched;

        public event Action<EnemyCharacter> PlayerDie;
        
        [Inject]
        public ServerPlayerConnector(MultiplayerManager multiplayerManager, PlayerProvider playerProvider, EnemyPool pool, ScoreManager scoreManager, SpawnPointManager spawnPointManager)
        {
            _multiplayerManager = multiplayerManager;
            _playerProvider = playerProvider;
            _enemyPool = pool;
            _scoreManager = scoreManager;
            _spawnPointManager = spawnPointManager;
        }

        public void Init()
        {
            _character = _playerProvider.GetCharacter();
            _playerGun = _playerProvider.GetGun();
            _health = _playerProvider.GetHealth();
            
            _character.OnMove += SendMessage;
            _playerGun.OnShootData += SendShoot;
            _playerGun.OnChangeWeapon += ChangeWeapon;
            _character.OnCrouch += CharacterCrouch;
            _multiplayerManager.OnPlayerCreate += PlayerCreated;
            _multiplayerManager.OnPlayerRestart += Restart;
            _scoreManager.OnPlayerWin += PlayerWin;
        }

        

        private void PlayerWin()
        {
            WinInfo winInfo = new WinInfo();
            winInfo.key = _multiplayerManager.ReturnSessionId();
            
            string jsonWin = JsonUtility.ToJson(winInfo);
            _multiplayerManager.SendMessage(Win, jsonWin);
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
                    
                    case "kills":
                        _scoreManager.UpdatePlayerKills((byte)change.Value);
                        Debug.Log($"PlayerKills: {(byte)change.Value}");
                        break;
                }
            }
            
        }
        
        private void ChangeWeapon(WeaponCollection obj)
        {
            SwapInfo swapInfo = new SwapInfo(_multiplayerManager.ReturnSessionId(), obj);
            
            string jsonData = JsonUtility.ToJson(swapInfo);
            _multiplayerManager.SendMessage(SwapWeapon, jsonData);
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
            Vector3 position = _spawnPointManager.GetRandomSpawnPoint();
            
            _character.Restart(position, enemy);
        }

        public void Dispose()
        {
            if (_character == null) 
                return;
            
            _character.OnMove -= SendMessage;
            _character.OnCrouch -= CharacterCrouch;
            _playerGun.OnShootData -= SendShoot;
            _player.OnChange -= OnChange;

        }
    }
}