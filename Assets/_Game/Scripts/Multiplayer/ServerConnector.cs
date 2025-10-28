using System;
using System.Collections.Generic;
using _Game.Scripts.Gun;
using _Game.Scripts.Providers;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Multiplayer
{
    public class ServerConnector : IDisposable
    {
        private MultiplayerManager _multiplayerManager;
        private PlayerProvider _playerProvider;
        
        private PlayerCharacter _player;
        private PlayerGun _playerGun;

        private const string ChangePosition = "move";
        private const string Shoot = "shoot";

        [Inject]
        public ServerConnector(MultiplayerManager multiplayerManager, PlayerProvider playerProvider)
        {
            _multiplayerManager = multiplayerManager;
            _playerProvider = playerProvider;
        }

        public void Init()
        {
            _player = _playerProvider.GetCharacter();
            _playerGun = _playerProvider.GetGun();
            
            _player.OnMove += SendMessage;
            _playerGun.OnShoot += SendShoot;
        }
        
        private void SendShoot(ShootInfo shootInfo)
        {
            shootInfo.key = _multiplayerManager.ReturnSessionId();
            string jsonShoot = JsonUtility.ToJson(shootInfo);
            
            _multiplayerManager.SendMessage(Shoot, jsonShoot);
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

        public void Dispose()
        {
            _player.OnMove -= SendMessage;
            _playerGun.OnShoot -= SendShoot;
        }
    }
}