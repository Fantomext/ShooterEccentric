using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts
{
    public class InputSystem : IDisposable
    {
        private MultiplayerManager _multiplayerManager;
        
        private PlayerInput _playerInput;
        private Character _player;

        private const string ChangePosition = "move";

        public event Action<Vector2> OnMove;

        public InputSystem(Character player, MultiplayerManager multiplayerManager)
        {
            _playerInput = new PlayerInput();
            _player = player;
            _multiplayerManager = multiplayerManager;
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
                    { "pX", position.x},
                    { "pY", position.y},
                    { "pZ", position.z},
                    
                    { "vX", 0},
                    { "vY", 0},
                    { "vZ", 0},

                };
            
            _multiplayerManager.SendMessage(ChangePosition, data);
        }
    }
}