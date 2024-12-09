using System.Collections.Generic;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;

public class StartPanelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private RectTransform _startPanel;
    [SerializeField] private RectTransform _lab1Panel;
    [SerializeField] private RectTransform _lab2Panel;
    [SerializeField] private List<RectTransform> _rulesPanels;

    [Space]
    [Header("Configs")]
    [SerializeField] private RectTransform _defaultPanel;

    [CanBeNull] private GameManager _gameManager;
    
    private RectTransform _currentRectTransform;
    private int _currentRuleNumber = 0;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        _startPanel.gameObject.SetActive(false);
        _lab1Panel.gameObject.SetActive(false);
        _lab2Panel.gameObject.SetActive(false);

        foreach (var rulePanel in _rulesPanels)
        {
            rulePanel.gameObject.SetActive(false);
        }
        
        _defaultPanel.gameObject.SetActive(true);
        
        _currentRectTransform = _defaultPanel;
    }

    public void OpenLab1Panel()
    {
        SwitchPanel(_lab1Panel);
    }
    
    public void OpenLab2Panel()
    {
        SwitchPanel(_lab2Panel);
    }

    public void OpenRulesPanel()
    {
        SwitchPanel(_rulesPanels[_currentRuleNumber]);
    }

    public void BeginLab1()
    {
        if (_gameManager == null)
        {
            return;
        }
        
        _gameManager.StartGame(ELab.Lab1);
        _gameManager.LoadScene(Game.LAB_1_SCENE_NAME);
    }
    
    public void BeginLab2()
    {
        if (_gameManager == null)
        {
            return;
        }
        
        _gameManager.StartGame(ELab.Lab2);
        _gameManager.LoadScene(Game.LAB_2_SCENE_NAME);
    }

    public void Return()
    {
        SwitchPanel(_startPanel);
    }

    public void NextRule()
    {
        if (_currentRuleNumber + 1 < _rulesPanels.Count)
        {
            return;
        }
        
        SwitchPanel(_rulesPanels[_currentRuleNumber++]);
    }
    
    public void PrevRule()
    {
        if (_currentRuleNumber - 1 < 0)
        {
            return;
        }
        
        SwitchPanel(_rulesPanels[_currentRuleNumber--]);
    }

    private void SwitchPanel(RectTransform panel)
    {
        _currentRectTransform.gameObject.SetActive(false);
        _currentRectTransform = panel;
        _currentRectTransform.gameObject.SetActive(true);
    }
}
