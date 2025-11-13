using System;
using System.Collections.Generic;
using _Game.Scripts.Gun.StateMachine.Enum;
using _Game.Scripts.Info;
using Colyseus.Schema;
using UnityEngine;

namespace _Game.Scripts
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyCharacter _character;
        [SerializeField] private EnemyGun _gun;
        [SerializeField] private EnemyVisual _enemyVisual;

        private Player _player;
        private List<float> _timeInterval = new List<float>() { 0, 0, 0, 0, 0 };
        private float _lastReceiveTime;

        Vector2 _rotation = Vector2.zero;

        public event Action<Vector3, Vector3, string> OnShoot;
        public event Action<int> OnUpdateKill;
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

        public void Init(string sessiondID, Player player)
        {
            _character.Init(sessiondID);
            
            ColorInfo color = JsonUtility.FromJson<ColorInfo>(player.color);
            
            _enemyVisual.SetColor(new Color(color.colorR / 255, color.colorG / 255, color.colorB / 255));
            _player = player;
            _player.OnChange += OnChange;
            _character.SetSpeed(player.speed);
            _character.SetMaxHealth(player.maxHP);
            
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
                    
                    case "kills":
                        if ((byte)dataChange.PreviousValue < (byte)dataChange.Value)
                        {
                            OnUpdateKill?.Invoke((byte)dataChange.Value);
                            Debug.Log($"EnemyKills: {dataChange.Value}");
                        }
                        
                        break;
                    
                    case "curHP":
                        if ((sbyte)dataChange.PreviousValue < (sbyte)dataChange.Value)
                            _character.RestoreHP((sbyte) dataChange.Value);
                        
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

            OnShoot?.Invoke(position, velocty, info.key);
        }
        
        public void Crouch(bool crouch)
        {
            _character.CrouchSet(crouch);
        }

        private void OnDestroy()
        { 
            _player.OnChange -= OnChange;
        }

        public void Restart()
        {
            _character.Restart();
        }

        public void SwapWeapon(WeaponCollection weapon)
        {
            _gun.SwapWeapon(weapon);
        }
    }
}