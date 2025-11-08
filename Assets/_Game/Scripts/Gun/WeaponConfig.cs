using UnityEngine;

namespace _Game.Scripts.Gun
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "configs/WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public float DelayBetweenShootsRifle;
        public float SpeedRifle;
        public int DamageRifle;
        
        public float DelayBetweenShootsMagicWand;
        public float SpeedMagicWand;
        public int DamageWand;

    }
}