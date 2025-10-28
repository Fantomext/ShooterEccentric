using System;

namespace _Game.Scripts.Gun
{
    public interface IPoolable<T>
    {
        public void Release();

        public event Action<T> OnRelease;
    }
}