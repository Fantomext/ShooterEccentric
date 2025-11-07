using System;
using _Game.Scripts.Factory;
using _Game.Scripts.Intefaces;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Spawners
{
    public class EnemySpawner : ISpawner<EnemyController>
    {
        [Inject] private GameObjectFactory<EnemyController> _enemyFactory;

        public event Action<EnemyController> OnSpawned;
        
        public EnemyController Spawn(EnemyController prefab)
        {
            var newEnemy = _enemyFactory.Create(prefab);
            OnSpawned?.Invoke(newEnemy);
            return newEnemy;
        }

        public EnemyController Spawn(EnemyController prefab, Vector3 position, Quaternion rotation)
        {
            var newEnemy = _enemyFactory.Create(prefab, position, rotation);
            OnSpawned?.Invoke(newEnemy);
            return newEnemy;
        }

        public EnemyController Spawn(EnemyController prefab, Transform parent)
        {
            var newEnemy = _enemyFactory.Create(prefab, parent);
            OnSpawned?.Invoke(newEnemy);
            return newEnemy;
        }


        

    }
}