using System;
using UnityEngine;

public class CheckFly : MonoBehaviour
{
    [field : SerializeField] public bool IsGrounded { get; set; }
    
    [SerializeField] private float _coyoteTime;
    [SerializeField] private float _raycastDistance = 0.7f;
    private float _flyTimer;
    
    private bool _prevIsGrounded;

    public event Action<bool> OnGroundedChanged; 

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0f,0.5f,0f), -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, LayerMask.GetMask("Ground")))
        {
            IsGrounded = true;
            _flyTimer = 0;
        }
        else
        {
            _flyTimer += Time.deltaTime;
            
            if (_flyTimer > _coyoteTime)
                IsGrounded = false;
        }

        if (_prevIsGrounded == IsGrounded) 
            return;
        
        OnGroundedChanged?.Invoke(IsGrounded);
        _prevIsGrounded = IsGrounded;
    }
}
