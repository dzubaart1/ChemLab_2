using System.Collections.Generic;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;

public class StartPanelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private RectTransform _startPanel;
    [SerializeField] private List<RectTransform> _rulesPanels;

    [Space]
    [Header("Configs")]
    [SerializeField] private RectTransform _defaultPanel;
    
    private RectTransform _currentRectTransform;
    private int _currentRuleNumber = 0;

    private void Start()
    {
        _startPanel.gameObject.SetActive(false);

        foreach (var rulePanel in _rulesPanels)
        {
            rulePanel.gameObject.SetActive(false);
        }
        
        _defaultPanel.gameObject.SetActive(true);
        
        _currentRectTransform = _defaultPanel;
    }

    public void OpenRulesPanel()
    {
        SwitchPanel(_rulesPanels[_currentRuleNumber]);
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

    public void Begin()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.LoadScene(GameManager.LOBBY_SCENE_NAME);
    }

    public void Return()
    {
        SwitchPanel(_startPanel);
    }

    public void NextRule()
    {
        if (_currentRuleNumber + 1 == _rulesPanels.Count)
        {
            return;
        }
        
        SwitchPanel(_rulesPanels[++_currentRuleNumber]);
    }
    
    public void PrevRule()
    {
        if (_currentRuleNumber - 1 < 0)
        {
            return;
        }
        
        SwitchPanel(_rulesPanels[--_currentRuleNumber]);
    }

    private void SwitchPanel(RectTransform panel)
    {
        _currentRectTransform.gameObject.SetActive(false);
        _currentRectTransform = panel;
        _currentRectTransform.gameObject.SetActive(true);
    }
}
