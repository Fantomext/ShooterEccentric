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
        
        public event Action<Bullet> OnRelease;

        public void Init(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity ;
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
            Release();
        }
    }
}