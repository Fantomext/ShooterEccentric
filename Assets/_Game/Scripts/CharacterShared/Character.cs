using System;
using UnityEngine;

namespace _Game.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        [field : SerializeField] public float Speed { get; protected set; }
        
        [field : SerializeField] public Vector3 Velocity { get; protected set; }
        
        [field: SerializeField] protected Transform _headTransform; 

        public event Action<float> OnSpeed;

        protected void OnSpeedChange()
        {
            Vector3 localVelocity = this.transform.InverseTransformVector(Velocity);
            float speed = localVelocity.magnitude / Speed;
            float sign = Mathf.Sign(localVelocity.z);
        
            OnSpeed?.Invoke(speed * sign);
        }
    }
}