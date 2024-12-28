using Core;
using UnityEngine;

public class LabSplitPanelController : MonoBehaviour
{
    [Header("Panels")] 
    [SerializeField] private RectTransform _startPanel;
    [SerializeField] private RectTransform _lab1Panel;
    [SerializeField] private RectTransform _lab2Panel;
    
    private RectTransform _currentRectTransform;
    
    private void Start()
    {
        _currentRectTransform = _startPanel;
    }

    public void BeginLab1()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.LoadScene(GameManager.LAB_1_SCENE_NAME);
    }
    
    public void BeginLab2()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.LoadScene(GameManager.LAB_2_SCENE_NAME);
    }
    
    public void BeginLab3()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.LoadScene(GameManager.LAB_3_SCENE_NAME);
    }

    public void Lab1Open()
    {
        SwitchPanel(_lab1Panel);
    }
    
    public void Lab2Open()
    {
        SwitchPanel(_lab2Panel);
    }

    public void Return()
    {
        SwitchPanel(_startPanel);
    }
    private void SwitchPanel(RectTransform panel)
    {
        _currentRectTransform.gameObject.SetActive(false);
        _currentRectTransform = panel;
        _currentRectTransform.gameObject.SetActive(true);
    }
}
