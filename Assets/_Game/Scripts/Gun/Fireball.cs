using System;

namespace _Game.Scripts.Gun
{
    public class Fireball : Bullet, IPoolable<Fireball>
    {
        public override void Release()
        {
            OnRelease?.Invoke(this);
        }

        public event Action<Fireball> OnRelease;
    }
}