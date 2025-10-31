using System;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace _Game.Scripts
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyCharacter _character;

        private Player _player;
        private List<float> _timeInterval = new List<float>() { 0, 0, 0, 0, 0 };
        private float _lastReceiveTime;

        Vector2 _rotation = Vector2.zero;

        public event Action<Vector3, Vector3> OnShoot;
        public event Action<bool> OnCrouch;

        private float AverageTimeInterval
        {
            get
            {
                float summ = 0;

                for (int i = 0; i < _timeInterval.Count; i++)
                    summ += _timeInterval[i];

                return summ / _timeInterval.Count;
            }
        }

        public void Init(Player player)
        {
            _player = player;
            _player.OnChange += OnChange;
            _character.SetSpeed(_player.speed);
        }

        private void SaveReceiveTime()
        {
            float interval = Time.time - _lastReceiveTime;
            _lastReceiveTime = Time.time;

            _timeInterval.Add(interval);
            _timeInterval.RemoveAt(0);
        }

        public void OnChange(List<DataChange> changes)
        {
            SaveReceiveTime();

            Vector3 position = _character.TargetPosition;
            Vector3 velocty = _character.Velocity;

            foreach (var dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "pX":
                        position.x = (float)dataChange.Value;
                        break;

                    case "pY":
                        position.y = (float)dataChange.Value;
                        break;

                    case "pZ":
                        position.z = (float)dataChange.Value;
                        break;

                    case "vX":
                        velocty.x = (float)dataChange.Value;
                        break;

                    case "vY":
                        velocty.y = (float)dataChange.Value;
                        break;

                    case "vZ":
                        velocty.z = (float)dataChange.Value;
                        break;

                    case "rX":
                        _rotation.x = (float)dataChange.Value;
                        break;

                    case "rY":
                        _rotation.y = (float)dataChange.Value;
                        break;
                    
                    default:
                        Debug.LogWarning($"{dataChange.Field} is not a valid field");
                        break;
                }
            }

            _character.SetMovement(position, velocty, AverageTimeInterval);
            _character.SetRotate(_rotation);
        }

        public void Shoot(ShootInfo info)
        {
            Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
            Vector3 velocty = new Vector3(info.dX, info.dY, info.dZ);

            OnShoot?.Invoke(position, velocty);
        }
        
        public void Crouch(bool crouch)
        {
            _character.CrouchSet(crouch);
        }

        private void OnDestroy()
        { 
            _player.OnChange -= OnChange;
        }
    }
}