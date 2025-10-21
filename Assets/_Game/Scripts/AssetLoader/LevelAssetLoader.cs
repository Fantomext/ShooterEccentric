using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace _Game.Scripts.AssetLoader
{
    public class LevelAssetLoader : IDisposable
    {
        private IObjectResolver _resolver;
        
        private List<AsyncOperationHandle<GameObject>> _handles = new List<AsyncOperationHandle<GameObject>>();

        [Inject]
        public LevelAssetLoader(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public async UniTask<T> Load<T>(string path) where T : MonoBehaviour
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
            await handle;
            
            var handleResult = handle.Result;
            
            _handles.Add(handle);

            T prefab = handleResult.GetComponent<T>();

            T instance = _resolver.Instantiate(prefab);
            
            _resolver.Inject(instance);

            return instance;
        }

        public async UniTask<T> LoadWithoutSpawn<T>(string path) where T : MonoBehaviour
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
            await handle;
            
            _handles.Add(handle);
            
            T prefab = handle.Result.GetComponent<T>();

            return prefab;
        }


        public void Dispose()
        {
            
            for (int i = 0; i < _handles.Count; i++)
                _handles[i].Release();
            
        }
    }
}