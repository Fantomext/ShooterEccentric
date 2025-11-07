using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth;
        
        private int _currentHealth;
        
        public event Action<int, int> OnChangeHealth;

        public void SetMax(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetCurrentHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
            OnChangeHealth?.Invoke(_maxHealth,_currentHealth);
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, _currentHealth, _maxHealth);

            OnChangeHealth?.Invoke(_maxHealth,_currentHealth);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            
            Debug.Log($"Damage: {damage}, Current Health: {_currentHealth}");
            
            OnChangeHealth?.Invoke(_maxHealth,_currentHealth);

            if (_currentHealth < 0 )
            {
                _currentHealth = 0;
                Debug.Log("Dead");
            }
        }
    }
}