using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Spawners;
using VContainer;

namespace _Game.Scripts
{
    public class EnemyPool : IDisposable
    {
        private readonly EnemySpawner _spawner;
        private List<EnemyCharacter> _enemies = new List<EnemyCharacter>();

        private const string Damage = "damage";

        [Inject]
        public EnemyPool(EnemySpawner spawner)
        {
            _spawner = spawner;
        }
        
        public void Init()
        {
            _spawner.OnSpawned += AddEnemy;
        }

        private void AddEnemy(EnemyController enemy)
        {
            _enemies.Add(enemy.GetComponent<EnemyCharacter>());
        }

        public EnemyCharacter GetEnemy(string sessionID)
        {
            return _enemies.First(enemy => enemy.SessionID == sessionID);
        }

        public void Dispose()
        {
            _spawner.OnSpawned -= AddEnemy;
        }
    }
}