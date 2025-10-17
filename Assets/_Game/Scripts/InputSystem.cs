using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts
{
    public class InputSystem : IDisposable
    {
        private PlayerInput _playerInput;
        private Character _player;

        public event Action<Vector2> OnMove;

        public InputSystem(Character player)
        {
            _playerInput = new PlayerInput();
            _player = player;
        }

        public void Initialize()
        {
            _playerInput.Enable();
            _playerInput.Player.Move.performed += Move;
            _playerInput.Player.Move.canceled += Move;
            _player.OnMove += SendMessage;
        }
        
        public void Dispose()
        {
            _playerInput.Disable();
            _playerInput.Player.Move.performed -= Move;
            _playerInput.Player.Move.canceled -= Move;
        }

        private void Move(InputAction.CallbackContext obj)
        {
            OnMove?.Invoke(_playerInput.Player.Move.ReadValue<Vector2>());
        }

        private void SendMessage(Vector3 position)
        {
            Dictionary<string, object> data
                = new Dictionary<string, object>()
                {
                    { "x", position.x},
                    { "y", position.z},

                };
            MultiplayerManager.Instance.SendMessage("move", data);
        }
        
    }
}