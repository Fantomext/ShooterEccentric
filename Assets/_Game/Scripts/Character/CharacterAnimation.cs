using _Game.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Animator _legAnimator;
    [SerializeField] private Animator _bodyAnimator;
    [SerializeField] private Character _playerCharacter;
    
    private const string Grounded = "Grounded";
    private const string Speed = "Speed";
    private const string Crouch = "Crouch";

    private void OnEnable()
    {
        _checkFly.OnGroundedChanged += GroundedChange;
        _playerCharacter.OnSpeed += PlayerMove;
        _playerCharacter.OnCrouch += SetCrouch;
    }

    

    private void OnDisable()
    {
        _checkFly.OnGroundedChanged -= GroundedChange;
        _playerCharacter.OnSpeed -= PlayerMove;
        _playerCharacter.OnCrouch -= SetCrouch;
    }
    
    private void SetCrouch(bool isCrouch)
    {
        _bodyAnimator.SetBool(Crouch, isCrouch);
    }

    private void PlayerMove(float speed)
    {
        _legAnimator.SetFloat(Speed, speed);
    }

    private void GroundedChange(bool groundedStay)
    {
        _legAnimator.SetBool(Grounded, groundedStay);
    }
}
