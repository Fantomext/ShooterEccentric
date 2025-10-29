using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gun
{
    public abstract class Gun : MonoBehaviour
    {
        [FormerlySerializedAs("_speed")] [SerializeField] protected float _bulletSpeed;
        public Action OnShoot;
    }
}
