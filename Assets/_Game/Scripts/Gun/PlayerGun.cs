using System;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Gun
{
    public class PlayerGun : MonoBehaviour
    {
        [Inject] private InputSystem _inputSystem;
        [Inject] private BulletPool _bulletPool;
        
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _delayBetweenShoots;

        private ShootInfo _shootInfo = new ShootInfo();
        
        private float _currentIntervalBetweenShoots;

        public event Action<ShootInfo> OnShoot;

        private void Update()
        {
            if (_inputSystem.IsShoot)
                TryShootStart();
                
        }

        public void TryShootStart()
        {
            if (_delayBetweenShoots > Time.time - _currentIntervalBetweenShoots)
                return;
            
            Vector3 position = _bulletPoint.position;
            Vector3 direction = _bulletPoint.forward;
            
            Bullet newBullet = _bulletPool.TakeBullet();
            newBullet.transform.position = position;
            newBullet.Init(direction, _bulletSpeed);
            
            direction *= _bulletSpeed;
            
            _currentIntervalBetweenShoots = Time.time;
                
            _shootInfo.pX = position.x;
            _shootInfo.pY = position.y;
            _shootInfo.pZ = position.z;
        
            _shootInfo.dX = direction.x;
            _shootInfo.dY = direction.y;
            _shootInfo.dZ = direction.z;

            OnShoot?.Invoke(_shootInfo);
        }
    }
}