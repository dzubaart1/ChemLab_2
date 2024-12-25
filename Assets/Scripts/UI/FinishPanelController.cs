using System.Collections.Generic;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;

public class FinishPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _resultsPanel;
    
    [CanBeNull] private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }
    public void ShowResultPanel()
    {
        _mainPanel.SetActive(false);
        _resultsPanel.SetActive(true);
    }

    public void Return()
    {
        _mainPanel.SetActive(true);
        _resultsPanel.SetActive(false);
    }

    public void ReturnToLobby()
    {
        _gameManager.LoadScene("LobbyScene");
    }
}
