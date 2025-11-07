using System;
using _Game.Scripts.Multiplayer;
using UnityEngine;

namespace _Game.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] protected Transform _headTransform; 
        [field: SerializeField] protected CapsuleCollider _collider; 
        [field: SerializeField] protected ModelVisualParts _visualParts; 
        
        [field : SerializeField] public float StayHeight { get; protected set; }
        [field : SerializeField] public float CrouchHeight { get; protected set; }
        [field : SerializeField] public Vector3 StayCenterCollider { get; protected set; }
        [field : SerializeField] public Vector3 CrouchCenterCollider { get; protected set; }
        [field : SerializeField] public Vector3 Velocity { get; protected set; }
        public string SessionID { get; set; }
        public int MaxHealth { get; protected set; }
        public float Speed { get; set; }

        [SerializeField] protected Health _health;

        public event Action<float> OnSpeed;
        public event Action<bool> OnCrouch;

        protected void OnSpeedChange()
        {
            Vector3 localVelocity = this.transform.InverseTransformVector(Velocity);
            float speed = localVelocity.magnitude / Speed;
            float sign = Mathf.Sign(localVelocity.z);
        
            OnSpeed?.Invoke(speed * sign);
        }

        public void CrouchSet(bool crouch)
        {
            if (crouch)
                Crouch();
            else
                UnCrouch();
        }

        protected void Crouch()
        {
            _collider.height = CrouchHeight;
            _collider.center = CrouchCenterCollider;
            OnCrouch?.Invoke(true);
        }

        protected void UnCrouch()
        {
            _collider.height = StayHeight;
            _collider.center = StayCenterCollider;
            OnCrouch?.Invoke(false);
        }

    }
}