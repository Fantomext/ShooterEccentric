using System;
using System.Collections.Generic;
using _Game.Scripts.Gun.StateMachine.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Gun
{
    public abstract class Gun : SerializedMonoBehaviour
    {
        [SerializeField] protected float _bulletSpeed;
        
        [DictionaryDrawerSettings(KeyLabel = "Тип оружия", ValueLabel = "Ссылка")]
        [SerializeField]
        protected Dictionary<WeaponCollection, GameObject> _weaponsVisual = new Dictionary<WeaponCollection, GameObject>();
        
        public Action OnShoot;
        public Action<WeaponCollection> OnChangeWeapon;

        public Dictionary<WeaponCollection, GameObject> WeaponsVisual => _weaponsVisual;
    }
}
