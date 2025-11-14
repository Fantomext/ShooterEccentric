using System;
using UnityEngine;

namespace _Game.Scripts.Gun
{
    public class Fireball : Bullet, IPoolable<Fireball>
    {
        public override void Release()
        {
            OnRelease?.Invoke(this);
        }
        
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out EnemyCharacter health))
            {
                health.TakeDamage(_damage, _playerId);
                _cts?.Cancel();
                Release();         }
        }

       
        public event Action<Fireball> OnRelease;
    }
}