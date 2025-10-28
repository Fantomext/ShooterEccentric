using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Gun
{
    public class BulletPool 
    {
        private readonly GameObjectPool<Bullet> _pool;
        private GameObject _parent;
        
        [Inject]
        public BulletPool(GameObjectPool<Bullet> pool)
        {
            _pool = pool;
        }

        public async UniTask Init()
        {
            _parent = new GameObject("BulletHolder");
            await _pool.OnLoadPrefab("Bullet", 100, _parent.transform);
        }

        public Bullet TakeBullet()
        {
            var bullet = _pool.GetFromPool();
            bullet.transform.parent = _parent.transform;
            
            return bullet;
        }
        
    }
}