using System;
using _Game.Scripts;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Animator _animator;
    [SerializeField] private Character _playerCharacter;
    
    private const string Grounded = "Grounded";
    private const string Speed = "Speed";

    private void OnEnable()
    {
        _checkFly.OnGroundedChanged += GroundedChange;
        _playerCharacter.OnSpeed += PlayerMove;
    }

    private void OnDisable()
    {
        _checkFly.OnGroundedChanged -= GroundedChange;
        _playerCharacter.OnSpeed -= PlayerMove;
    }

    private void PlayerMove(float speed)
    {
        _animator.SetFloat(Speed, speed);
    }

    private void GroundedChange(bool groundedStay)
    {
        _animator.SetBool(Grounded, groundedStay);
    }
}
