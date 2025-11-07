using UnityEngine;

namespace _Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "configs/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        public int MaxHealth;
        public int Speed;
    }
}