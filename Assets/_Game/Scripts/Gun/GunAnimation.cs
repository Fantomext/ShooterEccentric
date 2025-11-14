using System;
using System.Collections.Generic;
using _Game.Scripts.Gun.StateMachine.Enum;
using UnityEngine;

namespace _Game.Scripts.Gun
{
    public class GunAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Gun _playerGun;
        
        private Dictionary<WeaponCollection, GameObject> _weapons;

        private const string Shoot = "Shoot";

        private void Start()
        {
            _weapons = _playerGun.WeaponsVisual;
        }

        private void OnEnable()
        {
            _playerGun.OnShoot += Shooting;
            _playerGun.OnChangeWeapon += ChangeWeapon;
        }

        private void ChangeWeapon(WeaponCollection weapon)
        {
            if (_weapons.TryGetValue(weapon, out var value))
            {
                _animator = value.GetComponent<Animator>();
            }
        }

        private void OnDisable()
        {
            _playerGun.OnShoot -= Shooting;
            _playerGun.OnChangeWeapon -= ChangeWeapon;
        }

        private void Shooting()
        {
            _animator.SetTrigger(Shoot);
        }
    }
}