using System;
using UnityEngine;

namespace _Game.Scripts.Gun
{
    public abstract class Gun : MonoBehaviour
    {
        [SerializeField] private float _speed;
        public Action OnShoot;
    }
}
