using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class EnemyCharacter : Character
    {
        [HideInInspector] public Vector3 TargetPosition = Vector3.zero;
        private float _velocityMagnitude = 0;

        public void OnEnable()
        {
            
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }
        
        private void Update()
        {
            if (_velocityMagnitude > 0.1f)
            {
                float maxDistance = _velocityMagnitude * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
            }
            else
            {
                transform.position = TargetPosition;
            }
            
            OnSpeedChange();
        }

        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageTimeInterval)
        {
            TargetPosition = position + velocity * averageTimeInterval;
            
            Velocity = velocity;
            _velocityMagnitude = velocity.magnitude;
        }

        public void SetRotate(Vector2 rotation)
        {
            _headTransform.localEulerAngles = new Vector3(rotation.x , 0, 0);
            transform.eulerAngles = new Vector3(0f,rotation.y,0);
        }
    }
}