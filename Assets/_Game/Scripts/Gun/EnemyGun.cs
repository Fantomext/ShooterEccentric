using System;
using _Game.Scripts;
using _Game.Scripts.Gun;
using _Game.Scripts.Gun.StateMachine.Enum;
using UnityEngine;
using VContainer;

public class EnemyGun : Gun
{
    [SerializeField] private EnemyController _character;
    [Inject] private BulletPool _bulletPool;

    private Func<Bullet> _bulletGetter;

    private void Start()
    {
        SwapWeapon(WeaponCollection.Rifle);
    }

    private void OnEnable()
    {
        _character.OnShoot += Shoot;
    }

    private void OnDisable()
    {
        _character.OnShoot -= Shoot;
    }

    public void Shoot(Vector3 position, Vector3 velocity, string id)
    { 
        Bullet newBullet = _bulletGetter.Invoke();
        newBullet.transform.position = position;
        newBullet.Init(velocity, id);

        OnShoot?.Invoke();
    }

    public void SwapWeapon(WeaponCollection weapon)
    {
        foreach (var kvp in _weaponsVisual)
            kvp.Value.SetActive(false);
        
        switch (weapon)
        {
            case WeaponCollection.Rifle:
                _weaponsVisual[WeaponCollection.Rifle].SetActive(true);
                _bulletGetter = _bulletPool.TakeBullet;
                break;
            
            case WeaponCollection.MagicWand:
                _weaponsVisual[WeaponCollection.MagicWand].SetActive(true);
                _bulletGetter = _bulletPool.TakeFireball;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(weapon), weapon, null);
        }
        
        OnChangeWeapon?.Invoke(weapon);
    }
}
