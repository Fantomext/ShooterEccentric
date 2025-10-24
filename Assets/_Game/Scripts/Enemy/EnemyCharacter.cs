using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    public class EnemyCharacter : MonoBehaviour
    {
        [HideInInspector] public Vector3 TargetPosition;
        private float _velocityMagnitude;
        
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
        }

        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageTimeInterval)
        {
            TargetPosition = position + velocity * averageTimeInterval;
            _velocityMagnitude = velocity.magnitude;
        }
    }
}