using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts
{
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] protected float _lifeTime = 3f;
        [SerializeField] protected Rigidbody _rigidbody;
        protected int _damage;
        protected string _playerId;
        
        protected CancellationTokenSource _cts;
        
        public virtual void Init(Vector3 velocity, string playerId = null, int dmg = 0)
        {
            _rigidbody.linearVelocity = velocity;
            _damage = dmg;
            _playerId = playerId;
            _cts = new CancellationTokenSource();
            DelayDestroy().Forget();
        }

        public void SetPosition(Vector3 position)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.MovePosition(position);
        }

        protected async UniTask DelayDestroy()
        {
            await UniTask.WaitForSeconds(_lifeTime, true, cancellationToken: _cts.Token);
            Release();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DamageReceiver receiver))
                receiver.TakeDamage(_damage, _playerId);
            
            _cts?.Cancel();
        }
        public virtual void Release()
        {
            
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
