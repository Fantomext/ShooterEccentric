﻿using System;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Gun
{
    public class PlayerGun : Gun
    {
        [Inject] private InputSystem _inputSystem;
        [Inject] private BulletPool _bulletPool;
        
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private float _delayBetweenShoots;

        private ShootInfo _shootInfo = new ShootInfo();
        
        private float _currentIntervalBetweenShoots;

        public event Action<ShootInfo> OnShootData;

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
            Vector3 velocity = _bulletPoint.forward * _bulletSpeed;
            
            Bullet newBullet = _bulletPool.TakeBullet();
            newBullet.transform.position = position;
            newBullet.Init(velocity);
            
            _currentIntervalBetweenShoots = Time.time;
                
            _shootInfo.pX = position.x;
            _shootInfo.pY = position.y;
            _shootInfo.pZ = position.z;
        
            _shootInfo.dX = velocity.x;
            _shootInfo.dY = velocity.y;
            _shootInfo.dZ = velocity.z;

            OnShootData?.Invoke(_shootInfo);
            OnShoot?.Invoke();
        }
    }
}