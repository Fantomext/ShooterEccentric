using _Game.Scripts.AssetLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace _Game.Scripts.Gun
{
    public class GameObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly IObjectResolver _resolver;
        private readonly LevelAssetLoader _assetProvider;
        
        private T _poolPrefab;
        

        private IObjectPool<T> _pool;

        
        [Inject]
        public GameObjectPool(IObjectResolver resolver, LevelAssetLoader assetProvider)
        {
            _resolver = resolver;
            _assetProvider = assetProvider;
            
            _pool = new ObjectPool<T>(
                createFunc: CreatePoolObject,
                actionOnGet: OnGetFromPool,
                actionOnDestroy: OnDestroyPoolObject,
                actionOnRelease: OnReleasePoolObject,
                collectionCheck: true,
                defaultCapacity: 100,
                maxSize: 500

            );
        }

        ~GameObjectPool()
        {
            _pool.Clear();
        }
        
        public async UniTask OnLoadPrefab (string pathToPrefab, int countClones, Transform parent)
        {
            _poolPrefab = await _assetProvider.LoadWithoutSpawn<T>(pathToPrefab);

            for (int i = 0; i < countClones; i++)
            {
                var spawnClone = CreatePoolObject();
                spawnClone.transform.SetParent(parent);
            }
        }

        public T GetFromPool()
        {
            T poolObject = _pool.Get();
            
            poolObject.OnRelease += _pool.Release;
            
            return poolObject;
        }
        
        private T CreatePoolObject()
        {
            T newPoolObject = _resolver.Instantiate(_poolPrefab);
            
            newPoolObject.gameObject.SetActive(false);
            
            return newPoolObject;
        }
        
        private void OnGetFromPool(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleasePoolObject(T obj)
        {
            obj.OnRelease -= _pool.Release;
            
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(T obj)
        {
            //Уничтожается объект
        }
    }
}