using System;
using _Game.Scripts.Multiplayer;
using TMPro;
using UnityEngine;
using VContainer;

public class ScoreUI : MonoBehaviour
{
    [Inject] private ScoreManager _scoreManager;
    
    [SerializeField] private TMP_Text _text;

    private int _enemyLoss;
    private int _playerLoss;

    private void OnEnable()
    {
        _scoreManager.OnUpdateScore += SetScore;
    }

    private void OnDisable()
    {
        _scoreManager.OnUpdateScore -= SetScore;
    }

    public void SetScore(int enemyKills, int playerKills)
    {
        _text.text = $"{enemyKills} : {playerKills}";
    }

    
}
