using System;
using _Game.Scripts.Gun.StateMachine;
using _Game.Scripts.Gun.StateMachine.Enum;
using UnityEngine;
using VContainer;

namespace _Game.Scripts.Gun
{
    public class PlayerGun : Gun
    {
        [Inject] private InputSystem _inputSystem;
        [Inject] private BulletPool _bulletPool;
        [Inject] private WeaponConfig _config;
        
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private float _delayBetweenShoots;
        
        private GunState _gun;
        
        public string PlayerId { get; set; }

        public event Action<ShootInfo> OnShootData;
        

        public void SetId(string id)
        {
            PlayerId = id;
            _gun = new GunState(this, _bulletPool, _config);
            ChangeWeapon(WeaponCollection.Rifle);
        }

        private void OnEnable()
        {
            _inputSystem.OnChooseWeapon += ChangeWeapon;
            
        }

        private void OnDisable()
        {
            _inputSystem.OnChooseWeapon -= ChangeWeapon;
        }

        private void Update()
        {
            if (_inputSystem.IsShoot)
                _gun.TryShoot();
                
        }

        public void ShootSuccess(ShootInfo info)
        {
            info.key = PlayerId;
            OnShootData?.Invoke(info);
            OnShoot?.Invoke();
        }

        public Transform GetBulletPoint()
        {
            return _bulletPoint;
        }

        public void ChangeWeapon(WeaponCollection weaponCollection)
        {
            foreach (var VARIABLE in _weaponsVisual)
                VARIABLE.Value.SetActive(false);
            
            
            switch (weaponCollection)
            {
                case WeaponCollection.Rifle:
                    _gun.SwitchWeapon(WeaponCollection.Rifle);
                    _weaponsVisual[WeaponCollection.Rifle].SetActive(true);
                    OnChangeWeapon?.Invoke(WeaponCollection.Rifle);

                    break;
                case WeaponCollection.MagicWand:
                    _gun.SwitchWeapon(WeaponCollection.MagicWand);
                    _weaponsVisual[WeaponCollection.MagicWand].SetActive(true);
                    OnChangeWeapon?.Invoke(WeaponCollection.MagicWand);
                    break;
            }
            
        }

    }
}