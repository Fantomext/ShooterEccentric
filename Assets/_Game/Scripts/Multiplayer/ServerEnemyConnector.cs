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
        private readonly ScoreManager _scoreManager;
        
        private List<EnemyCharacter> _enemies = new List<EnemyCharacter>();
        private List<EnemyController> _enemiesControllers = new List<EnemyController>();

        private const string Damage = "damage";

        [Inject]
        public ServerEnemyConnector(EnemySpawner spawner, MultiplayerManager multiplayerManager, ScoreManager scoreManager)
        {
            _spawner = spawner;
            _multiplayerManager = multiplayerManager;
            _scoreManager = scoreManager;
        }

        public void Init()
        {
            _spawner.OnSpawned += AddEnemy;
        }

        public void AddEnemy(EnemyController enemy)
        {
            _enemies.Add(enemy.GetComponent<EnemyCharacter>());
            _enemiesControllers.Add(enemy);
            
            _enemies[^1].OnTakeDamage += SendTakeDamage;
            _enemiesControllers[^1].OnUpdateKill += UpdateKill;
        }

        private void UpdateKill(int count)
        {
            _scoreManager.UpdateEnemyKills(count);
        }

        private void SendTakeDamage(Dictionary<string, object> data)
        {
            _multiplayerManager.SendMessage(Damage, data); 
        }

        public void Dispose()
        {
            _spawner.OnSpawned -= AddEnemy;

            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].OnTakeDamage -= SendTakeDamage;
                _enemiesControllers[i].OnUpdateKill -= UpdateKill;
            }
            
        }
    }
}