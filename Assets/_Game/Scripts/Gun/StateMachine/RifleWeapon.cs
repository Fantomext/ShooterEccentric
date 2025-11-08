using _Game.Scripts.Intefaces;
using UnityEngine;

namespace _Game.Scripts.Gun.StateMachine
{
    public class RifleWeapon : IWeapon
    {
        private PlayerGun _playerGun;
        private ShootInfo _shootInfo = new ShootInfo();
        private BulletPool _bulletPool;

        private float _delayBetweenShoots;
        private float _currentIntervalBetweenShoots;
        private float _speedProjectile;
        private int _damage;
        private string _playerID;

        
        private Transform _bulletPoint;
        
        public RifleWeapon(PlayerGun playerGun, BulletPool bulletPool, WeaponConfig config, string playerID)
        {
            _playerGun = playerGun;
            _bulletPoint = _playerGun.GetBulletPoint();
            _bulletPool = bulletPool;
            _currentIntervalBetweenShoots = _delayBetweenShoots;
            
            _delayBetweenShoots = config.DelayBetweenShootsRifle;
            _speedProjectile = config.SpeedRifle;
            _damage = config.DamageRifle;
            _playerID = playerID;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }

        public void Shoot()
        {
            if (_delayBetweenShoots > Time.time - _currentIntervalBetweenShoots)
                return;
            
            _currentIntervalBetweenShoots = Time.time;
            
            Vector3 position = _bulletPoint.position;
            Vector3 velocity = _bulletPoint.forward * _speedProjectile;
            
            Bullet newBullet = _bulletPool.TakeBullet();
            newBullet.transform.position = position;
            newBullet.Init(velocity, _playerID, _damage);
                
            _shootInfo.pX = position.x;
            _shootInfo.pY = position.y;
            _shootInfo.pZ = position.z;
        
            _shootInfo.dX = velocity.x;
            _shootInfo.dY = velocity.y;
            _shootInfo.dZ = velocity.z;
            
            _playerGun.ShootSuccess(_shootInfo);
        }
    }
}