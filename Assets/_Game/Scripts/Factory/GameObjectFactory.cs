using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Scripts.Factory
{
    public class GameObjectFactory<T> where T : MonoBehaviour
    {
        private readonly IObjectResolver _resolver;
        
        public event Action<MonoBehaviour> OnCreate;

        [Inject]
        public GameObjectFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T Create<T>(T prefab) where T : MonoBehaviour
        {
            T instance;

            instance = _resolver.Instantiate(prefab);
            
            OnCreate?.Invoke(instance);
            
            return instance;
        }

        public T Create<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            T instance;
            
            instance = _resolver.Instantiate(prefab, position, rotation);
            
            OnCreate?.Invoke(instance);
            
            return instance;
        }

        public T Create<T>(T prefab, Transform parent) where T : MonoBehaviour
        {
            T instance;

            instance = _resolver.Instantiate(prefab, parent);
            
            OnCreate?.Invoke(instance);

            return instance;
        }
    }
}