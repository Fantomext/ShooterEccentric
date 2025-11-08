using System;
using System.Collections.Generic;
using _Game.Scripts.Gun.StateMachine.Enum;
using _Game.Scripts.Intefaces;
using UnityEngine;

namespace _Game.Scripts.Gun.StateMachine
{
    public class GunState
    {
        private readonly PlayerGun _playerGun;
        
        private RifleWeapon _rifleWeapon;
        private MagicWand _wandWeapon;
        
        private IWeapon _currentWeapon;

        public GunState(PlayerGun gun, BulletPool bulletPool, WeaponConfig config)
        {
            _playerGun = gun;
            
            _rifleWeapon = new RifleWeapon(_playerGun, bulletPool, config, _playerGun.PlayerId);
            _wandWeapon = new MagicWand(_playerGun, bulletPool, config, _playerGun.PlayerId);
        }

        public void SwitchWeapon(WeaponCollection weapon)
        {
            _currentWeapon?.Exit();
            
            switch (weapon)
            {
                case WeaponCollection.Rifle:
                    _currentWeapon = _rifleWeapon;
                    break;
                
                case WeaponCollection.MagicWand:
                    _currentWeapon = _wandWeapon;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(weapon), weapon, null);
            }
            
            _currentWeapon?.Enter();
        }

        public void TryShoot()
        {
            _currentWeapon.Shoot();
        }
    }
}