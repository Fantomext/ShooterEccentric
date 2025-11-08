using UnityEngine;
using VContainer;

public class EndGameUI : MonoBehaviour
{
    [Inject] private MultiplayerManager _multiplayerManager;
    
    [SerializeField] private GameObject _gameWinPanel;
    [SerializeField] private GameObject _gameLosePanel;

    private void OnEnable()
    {
        _multiplayerManager.OnGameEnd += EndScreen;
    }

    private void EndScreen(bool win)
    {
        if (win)
        {
            _gameWinPanel.SetActive(true);
        }
        else
        {
            _gameLosePanel.SetActive(true);
        }
    }

}
