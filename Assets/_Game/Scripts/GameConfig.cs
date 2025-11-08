using UnityEngine;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "configs/GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public int CountToWin { get; set; }
    }
}