using System;
using _Game.Scripts;
using UnityEngine;
using VContainer;

public class Character : MonoBehaviour
{
    [Inject] private MultiplayerManager _multiplayerManager;
    
    [SerializeField] private float _speed = 2f;
    
    private InputSystem _inputSystem;
    private Vector3 _direction;
    
    public event Action<Vector3> OnMove;

    private void Awake()
    {
        _inputSystem = new InputSystem(this, _multiplayerManager);
        _inputSystem.Initialize();
    }

    private void OnEnable()
    {
        _inputSystem.OnMove += ChangeDirection;
    }

    private void OnDisable()
    {
        _inputSystem.OnMove -= ChangeDirection;
    }

    private void Update()
    {
        //Move();
    }

    private void ChangeDirection(Vector2 input)
    {
        _direction = new Vector3(input.x, 0, input.y).normalized;
    }

    private void Move()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

   
}
