using System;
using System.Collections.Generic;
using _Game.Scripts.Spawners;
using VContainer;

namespace _Game.Scripts.Multiplayer
{
    public class ServerEnemyConnector : IDisposable
    {
        private readonly MultiplayerManager _multiplayerManager;
        private readonly EnemySpawner _spawner;
        
        private List<EnemyCharacter> _enemies = new List<EnemyCharacter>();

        private const string Damage = "damage";

        [Inject]
        public ServerEnemyConnector(EnemySpawner spawner, MultiplayerManager multiplayerManager)
        {
            _spawner = spawner;
            _multiplayerManager = multiplayerManager;
        }

        public void Init()
        {
            _spawner.OnSpawned += AddEnemy;
        }

        public void AddEnemy(EnemyController enemy)
        {
            _enemies.Add(enemy.GetComponent<EnemyCharacter>());
            
            _enemies[^1].OnTakeDamage += SendTakeDamage;
        }

        private void SendTakeDamage(Dictionary<string, object> data)
        {
            _multiplayerManager.SendMessage(Damage, data); 
        }

        public void Dispose()
        {
            _spawner.OnSpawned -= AddEnemy;

            for (int i = 0; i < _enemies.Count; i++)
                _enemies[i].OnTakeDamage -= SendTakeDamage;
            
        }
    }
}