using UnityEngine;

namespace _Game.Scripts
{
    public class EnemyCharacter : MonoBehaviour
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}