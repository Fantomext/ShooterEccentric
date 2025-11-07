using System;
using _Game.Scripts.Gun;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts
{
    public class Bullet : MonoBehaviour, IPoolable<Bullet>
    {
        [SerializeField] private float _lifeTime = 3f;
        [SerializeField] private Rigidbody _rigidbody;
        private int _damage;
        private string _playerId;
        
        public event Action<Bullet> OnRelease;

        public void Init(Vector3 velocity, string playerId = null, int dmg = 0)
        {
            _rigidbody.linearVelocity = velocity;
            _damage = dmg;
            _playerId = playerId;
            DelayDestroy().Forget();
        }

        public void Release()
        {
            OnRelease?.Invoke(this);
        }

        private async UniTask DelayDestroy()
        {
            await UniTask.WaitForSeconds(_lifeTime, true);
            Release();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out EnemyCharacter health))
                health.TakeDamage(_damage, _playerId);
            
            Release();
        }
    }
}