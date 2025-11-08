using System;

namespace _Game.Scripts.Gun
{
    public class BulletRifle : Bullet, IPoolable<BulletRifle>
    {
        public override void Release()
        {
            OnRelease?.Invoke(this);
        }

        public event Action<BulletRifle> OnRelease;
    }
}