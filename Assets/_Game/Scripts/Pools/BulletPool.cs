using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Gun
{
    public class BulletPool 
    {
        private readonly GameObjectPool<BulletRifle> _pool;
        private readonly GameObjectPool<Fireball> _poolFire;
        private GameObject _bulletPoolParent;
        private GameObject _fireballPoolParent;
        
        [Inject]
        public BulletPool(GameObjectPool<BulletRifle> pool, GameObjectPool<Fireball> poolFire)
        {
            _pool = pool;
            _poolFire = poolFire;
        }

        public async UniTask Init()
        {
            _bulletPoolParent = new GameObject("BulletHolder");
            _fireballPoolParent = new GameObject("FireBallHolder");
            await _pool.OnLoadPrefab("Bullet", 100, _bulletPoolParent.transform);
            await _poolFire.OnLoadPrefab("Fireball", 100, _bulletPoolParent.transform);
        }

        public Bullet TakeBullet()
        {
            var bullet = _pool.GetFromPool();
            bullet.transform.parent = _bulletPoolParent.transform;
            
            return bullet;
        }

        public Fireball TakeFireball()
        {
            var fireball = _poolFire.GetFromPool();
            fireball.transform.parent = _fireballPoolParent.transform;
            
            return fireball;
        }
        
    }
}