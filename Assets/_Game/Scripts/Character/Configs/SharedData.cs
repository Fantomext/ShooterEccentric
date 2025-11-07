using UnityEngine;

namespace _Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "SharedData", menuName = "configs/SharedData", order = 0)]
    public class SharedData : ScriptableObject
    {
        public float RespawnTime;
    }
}