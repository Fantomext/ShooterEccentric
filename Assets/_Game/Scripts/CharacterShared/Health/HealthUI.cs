using UnityEngine;

namespace _Game.Scripts
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] protected Health _health;
        [SerializeField] protected RectTransform _healthBar;
        
        [SerializeField] protected float _defaultWidth;

        private void OnValidate()
        {
            _defaultWidth = _healthBar.sizeDelta.x;
        }
        
        private void OnEnable()
        {
            _health.OnChangeHealth += UpdateHealth;
        }

        private void OnDisable()
        {
            _health.OnChangeHealth -= UpdateHealth;
        }

        public void UpdateHealth(int max, int current)
        {
            float percent = (float) current / (float)max;
            Debug.Log(percent);
            _healthBar.sizeDelta = new Vector2(_defaultWidth * percent, _healthBar.sizeDelta.y);
        }
    }
}