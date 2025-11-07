using System;
using UnityEngine;

namespace _Game.Scripts.Intefaces
{
    public interface ISpawner<T>
    {
        public T Spawn(T prefab);
        
        public T Spawn(T prefab, Vector3 position, Quaternion rotation);

        public T Spawn(T prefab, Transform parent);
        
        public event Action<T> OnSpawned;
    }
}