using System;
using UnityEngine;

namespace _Game.Scripts.Gun
{
    public class BulletRifle : Bullet, IPoolable<BulletRifle>
    {
        public override void Release()
        {
            OnRelease?.Invoke(this);
        }

        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
            
            Release();
        }

        public event Action<BulletRifle> OnRelease;
    }
}