using UnityEngine;

namespace _Game.Scripts
{
    public class DamageReceiver : MonoBehaviour
    {
        [SerializeField] private float _multiplier;
        [SerializeField] private EnemyCharacter _character;

        public void TakeDamage(float damage, string playerId)
        {
            int multiplierDamage = (int)(damage * _multiplier);
            _character.TakeDamage(multiplierDamage, playerId);
        }
        
    }
}