using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace _Game.Scripts
{
    public class InputSystem : IDisposable
    {
        private MultiplayerManager _multiplayerManager;
        
        private PlayerInput _playerInput;

        public event Action<Vector2> OnMove;

        public InputSystem(Character player, MultiplayerManager multiplayerManager)
        {
            _playerInput = new PlayerInput();
            _multiplayerManager = multiplayerManager;
        }

        public void Initialize()
        {
            _playerInput.Enable();
            _playerInput.Player.Move.performed += Move;
            _playerInput.Player.Move.canceled += Move;
        }
        
        public void Dispose()
        {
            _playerInput.Disable();
            _playerInput.Player.Move.performed -= Move;
            _playerInput.Player.Move.canceled -= Move;
        }

        private void Move(InputAction.CallbackContext obj)
        {
            Vector3 input = _playerInput.Player.Move.ReadValue<Vector2>();
            OnMove?.Invoke(input);
            SendMessage(new Vector3(input.x, 0, input.y));
        }

        private void SendMessage(Vector3 position)
        {
            Dictionary<string, object> data
                = new Dictionary<string, object>()
                {
                    { "x", position.x},
                    { "y", position.z},

                };
            _multiplayerManager.SendMessage("move", data);
        }
    }
}