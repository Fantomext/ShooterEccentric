using System;
using _Game.Scripts.Providers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts
{
    public class InputSystem : IDisposable
    {
        private readonly PlayerInput _playerInput;
        
        private PlayerCharacter _player;
        
        public bool IsShoot { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsAiming { get; private set; }
        public bool IsCrouching { get; private set; }
        
        private float _mouseSensitivity = 0.2f;

        public event Action<Vector2> OnMove;
        public event Action<Vector2> OnCameraDirectionChanged;
        public event Action OnJump;
        public event Action OnShootStart;
        public event Action OnShootEnd;

        public event Action OnCrouchStart;
        public event Action OnCrouchEnd;
        
        public InputSystem(PlayerProvider provider, MultiplayerManager multiplayerManager)
        {
            _playerInput = new PlayerInput();
        }

        public void Initialize()
        {
            _playerInput.Enable();
            
            _playerInput.Player.Move.performed += Move;
            _playerInput.Player.Move.canceled += Move;
            
            _playerInput.Player.Mouse.performed += CameraInput;
            _playerInput.Player.Mouse.canceled += CameraInput;

            _playerInput.Player.Jump.performed += Jump;

            _playerInput.Player.Shoot.performed += Shoot;
            _playerInput.Player.Shoot.canceled += ShootEnd;
            
            _playerInput.Player.Crouch.performed += CrouchStart;
            _playerInput.Player.Crouch.canceled += CrouchEnd;
        }

       

        public void Dispose()
        {
            _playerInput.Disable();
            
            _playerInput.Player.Move.performed -= Move;
            _playerInput.Player.Move.canceled -= Move;
            
            _playerInput.Player.Mouse.performed -= CameraInput;
            _playerInput.Player.Mouse.canceled -= CameraInput;
            
            _playerInput.Player.Crouch.performed -= CrouchStart;
            _playerInput.Player.Crouch.canceled -= CrouchEnd;
        }
        
        private void CrouchStart(InputAction.CallbackContext obj)
        {
            IsCrouching = true;
            OnCrouchStart?.Invoke();
        }
        
        private void CrouchEnd(InputAction.CallbackContext obj)
        {
            IsCrouching = false;
            OnCrouchEnd?.Invoke();
        }
        
        private void Shoot(InputAction.CallbackContext obj)
        {
            IsShoot = true;
            OnShootStart?.Invoke();
        }
        
        private void ShootEnd(InputAction.CallbackContext obj)
        {
            IsShoot = false;
            OnShootEnd?.Invoke();
        }

        private void CameraInput(InputAction.CallbackContext obj)
        {
            OnCameraDirectionChanged?.Invoke(_playerInput.Player.Mouse.ReadValue<Vector2>() * _mouseSensitivity);
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            OnJump?.Invoke();
        }

        private void Move(InputAction.CallbackContext obj)
        {
            OnMove?.Invoke(_playerInput.Player.Move.ReadValue<Vector2>());
        }

    }
}