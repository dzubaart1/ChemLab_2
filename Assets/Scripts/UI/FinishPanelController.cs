using Core;
using UnityEngine;

public class FinishPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _resultsPanel;

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
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        if (gameManager.CurrentBaseLocalManager == null)
        {
            return;
        }
        
        gameManager.LoadScene(GameManager.LOBBY_SCENE_NAME);
    }
}
