using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts
{
    public class EnemyCharacter : Character
    {
        [HideInInspector] public Vector3 TargetPosition = Vector3.zero;
        private float _velocityMagnitude = 0;
        
        private Vector3 _targetRotation = Vector3.zero;

        public event Action<Dictionary<string, object>> OnTakeDamage;

        public void Init(string sessionID)
        {
            SessionID = sessionID;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetMaxHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
            _health.SetMax(maxHealth);
            _health.SetCurrentHealth(maxHealth);
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
            Rotate();
        }

        public void TakeDamage(int damage, string enemyId)
        {
            _health.TakeDamage(damage);

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "id", SessionID },
                { "value", damage },
                { "enId", enemyId}

            };

            OnTakeDamage?.Invoke(data);
        }
        
        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageTimeInterval)
        {
            TargetPosition = position + velocity * averageTimeInterval;
            
            Velocity = velocity;
            _velocityMagnitude = velocity.magnitude;
        }
        
        public void Rotate()
        {
            if (_targetRotation.magnitude > 0)
            {
                _headTransform.localRotation = Quaternion.Lerp(_headTransform.localRotation, Quaternion.Euler(_targetRotation.x, _headTransform.localEulerAngles.y,_headTransform.localEulerAngles.z),  Time.deltaTime * 15);
                transform.rotation = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(transform.eulerAngles.x,_targetRotation.y,transform.eulerAngles.z),  Time.deltaTime * 15);
            }
        }
        
        public void SetRotate(Vector2 rotation)
        {
            _targetRotation = rotation;
        }

        public async void Restart()
        {
            _visualParts.HideModel();
            _collider.enabled = false;

            await UniTask.WaitForSeconds(3.1f);
            
            _collider.enabled = true;
            _visualParts.ShowModel();
        }

        public void RestoreHP(int newValue)
        {
            _health.SetCurrentHealth(newValue);
        }

    }
}