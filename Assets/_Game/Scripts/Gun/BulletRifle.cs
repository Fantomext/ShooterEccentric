using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Gun
{
    public class BulletRifle : Bullet, IPoolable<BulletRifle>
    {
        [SerializeField] private TrailRenderer _trail;

        public override async void Init(Vector3 velocity, string playerId = null, int dmg = 0)
        {
            base.Init(velocity, playerId, dmg);
            await UniTask.WaitForSeconds(0.05f);
            _trail.emitting = true;
        }

        public override void Release()
        {
            OnRelease?.Invoke(this);
            _trail.emitting = false;
            _trail.Clear();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            Release();
        }

        public event Action<BulletRifle> OnRelease;

    }
}