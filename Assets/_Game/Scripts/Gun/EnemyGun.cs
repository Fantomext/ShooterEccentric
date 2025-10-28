using _Game.Scripts;
using _Game.Scripts.Gun;
using UnityEngine;
using VContainer;

public class EnemyGun : Gun
{
    [SerializeField] private EnemyController _character;
    [Inject] private BulletPool _bulletPool;

    private void OnEnable()
    {
        _character.OnShoot += Shoot;
    }

    private void OnDisable()
    {
        _character.OnShoot -= Shoot;
    }

    public void Shoot(Vector3 position, Vector3 velocity)
    { 
        var newBullet = _bulletPool.TakeBullet();
        newBullet.transform.position = position;
        newBullet.Init(velocity);

        OnShoot?.Invoke();
    }
}
