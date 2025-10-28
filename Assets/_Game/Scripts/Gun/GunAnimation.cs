using UnityEngine;

namespace _Game.Scripts.Gun
{
    public class GunAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerGun _playerGun;

        private const string Shoot = "Shoot";

        private void OnEnable()
        {
            _playerGun.OnShoot += Shooting;
        }

        private void OnDisable()
        {
            _playerGun.OnShoot -= Shooting;
        }

        private void Shooting(ShootInfo shootInfo)
        {
            _animator.SetTrigger(Shoot);
        }
    }
}