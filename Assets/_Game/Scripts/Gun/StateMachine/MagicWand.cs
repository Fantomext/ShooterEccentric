using _Game.Scripts.Intefaces;
using UnityEngine;

namespace _Game.Scripts.Gun.StateMachine
{
    public class MagicWand : IWeapon
    {
        private PlayerGun _playerGun;
        private ShootInfo _shootInfo = new ShootInfo();
        private BulletPool _bulletPool;
        
        private Transform _bulletPoint;

        private float _delayBetweenShoots;
        private float _currentIntervalBetweenShoots;
        private float _speedProjectile;
        private int _damage;
        private string _playerID;
        

        public MagicWand(PlayerGun playerGun, BulletPool bulletPool, WeaponConfig config, string playerID)
        {
            _playerGun = playerGun;
            _bulletPoint = _playerGun.GetBulletPoint();
            _bulletPool = bulletPool;
            
            _delayBetweenShoots = config.DelayBetweenShootsMagicWand;
            _currentIntervalBetweenShoots = _delayBetweenShoots;
            _speedProjectile = config.SpeedMagicWand;
            _damage = config.DamageWand;
            _playerID = playerID;
        }

        public void Enter()
        {
           
        }

        public void Exit()
        {
        }

        public async void Shoot()
        {
            if (_delayBetweenShoots > Time.time - _currentIntervalBetweenShoots)
                return;
            
            _currentIntervalBetweenShoots = Time.time;

            for (int i = 0; i < 6; i++)
            {
                Vector3 position = _bulletPoint.position;
                Vector3 velocity = (_bulletPoint.forward + _bulletPoint.right * Random.Range(-0.05f,0.05f) + _bulletPoint.up * Random.Range(-0.05f,0.05f))  * _speedProjectile;
            
                Fireball newBullet = _bulletPool.TakeFireball();
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
}