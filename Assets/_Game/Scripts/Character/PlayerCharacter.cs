using System;
using _Game.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

public class PlayerCharacter : Character
{
    [Inject] private PlayerCamera _camera;
    [Inject] private InputSystem _inputSystem;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Transform _cameraPoint;
    
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private float _minHeadAnge;
    [SerializeField] private float _maxHeadAnge;
    [SerializeField] private float _jumpDelay = 0.2f;
    
    private Vector3 _moveDirection;
    private Vector3 _mouseDirection;

    private float _lastJumpTime;
    
    public event Action<Vector3, Vector3, Vector2> OnMove;

    private void Awake()
    {
        _camera.transform.parent = _cameraPoint.transform;
        _camera.transform.localPosition = Vector3.zero;
        _camera.transform.localRotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        InputActivate();
    }

    private void OnDisable()
    {
        InputDeactivate();
    }


    public void MoveToPoint(Vector3 point, Quaternion rotation)
    {
        _rigidbody.Move(point, rotation);
    }
    private void InputActivate()
    {
        _inputSystem.OnMove += ChangeDirection;
        _inputSystem.OnCameraDirectionChanged += CameraChangeDirection;
        _inputSystem.OnJump += Jump;
        
        _inputSystem.OnCrouchStart += Crouch;
        _inputSystem.OnCrouchEnd += UnCrouch;
    }

    private void InputDeactivate()
    {
        _inputSystem.OnMove -= ChangeDirection;
        _inputSystem.OnCameraDirectionChanged -= CameraChangeDirection;
        _inputSystem.OnJump -= Jump;
        
        _inputSystem.OnCrouchStart -= Crouch;
        _inputSystem.OnCrouchEnd -= UnCrouch;
    }
    
    private void Update()
    {
        RotateCamera();
        SendInfo();
        OnSpeedChange();

    }

    private void FixedUpdate()
    {
        Move();
        RotateBody();

    }
    
    private void Jump()
    {
        float deltaTime = Time.time - _lastJumpTime;

        if (deltaTime > _jumpDelay && _checkFly.IsGrounded)
        {
            _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
            _lastJumpTime = Time.time;
        }
    }

    private void SendInfo()
    {
        OnMove?.Invoke(transform.position, Velocity, new Vector2(_headTransform.localEulerAngles.x, transform.eulerAngles.y));
    }

    private void ChangeDirection(Vector2 input)
    {
        _moveDirection = new Vector3(input.x, 0, input.y).normalized;
    }
    
    private void CameraChangeDirection(Vector2 mouseInput)
    {
        _mouseDirection = mouseInput;
    }

    private void Move()
    {
        Vector3 velocity = (transform.forward * _moveDirection.z + transform.right * _moveDirection.x) * Speed; 
        velocity.y = _rigidbody.linearVelocity.y;
        Velocity = velocity;
        
        _rigidbody.linearVelocity = velocity;
        
        //Debug.Log($"Linear velocity: {velocity}");
    }

    private void RotateBody()
    {
        transform.rotation *= Quaternion.Euler(0f, _mouseDirection.x , 0f);
    }

    private void RotateCamera()
    {
        Vector3 eueler = _headTransform.localEulerAngles;
        eueler.x = eueler.x > 180 ? eueler.x - 360 : eueler.x;
        
        eueler.x = Mathf.Clamp(eueler.x - _mouseDirection.y, _minHeadAnge, _maxHeadAnge);
        
       _headTransform.transform.localEulerAngles = new Vector3(eueler.x, eueler.y,eueler.z);
    }
    

    public async void Restart(Vector3 position, Quaternion rotation, EnemyCharacter enemy)
    {
        _camera.ShowDeathCamera(enemy.transform);
        await UniTask.WaitForSeconds(1);
        _visualParts.HideModel();
        _inputSystem.BlockInput();
        
        ResetVelocity();
        
        await _rigidbody.DOMove(new Vector3(position.x, position.y, position.z), 1.5f).AsyncWaitForCompletion();

        _rigidbody.rotation = rotation;
        OnMove?.Invoke(new Vector3(position.x, position.y, position.z), Vector3.one, _rigidbody.transform.eulerAngles);
        
        await _camera.HideDeathCamera();
        _inputSystem.UnblockInput();
        _visualParts.ShowModel();
        
        
    }

    public void ResetVelocity()
    {
        ChangeDirection(Vector3.zero);
        CameraChangeDirection(Vector3.zero);
        
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
}
