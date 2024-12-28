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

    public void LoadLab1()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.OnChooseLab(ELab.Lab1);
    }
    
    public void LoadLab2()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.OnChooseLab(ELab.Lab2);
    }
    
    public void LoadLab3()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.OnChooseLab(ELab.Lab3);
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
