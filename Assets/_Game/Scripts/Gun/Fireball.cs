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
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DamageReceiver receiver))
            {
                receiver.TakeDamage(_damage, _playerId);
                _cts?.Cancel();
                Release();         
            }
        }

       
        public event Action<Fireball> OnRelease;
    }
}