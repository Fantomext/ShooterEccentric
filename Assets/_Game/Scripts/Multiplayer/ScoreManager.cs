using System;
using VContainer;

namespace _Game.Scripts.Multiplayer
{
    public class ScoreManager
    {
        [Inject] private GameConfig _gameConfig;
        public int _maxEnemyKills = 0;
        public int _playerKills = 0;

        public event Action<int, int> OnUpdateScore;
        public event Action OnPlayerWin;

        public void UpdateEnemyKills(int killsCount)
        {
            if (killsCount > _maxEnemyKills)
                _maxEnemyKills = killsCount;
            
            OnUpdateScore?.Invoke(_maxEnemyKills, _playerKills);
        }

        public void UpdatePlayerKills(int killsCount)
        {
            _playerKills = killsCount;
            OnUpdateScore?.Invoke(_maxEnemyKills, _playerKills);

            if (_gameConfig.CountToWin < _playerKills)
                OnPlayerWin?.Invoke();
        }

    }
}