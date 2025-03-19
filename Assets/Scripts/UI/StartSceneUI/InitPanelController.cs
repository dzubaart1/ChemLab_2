using System.Collections.Generic;
using Core;
using UnityEngine;

public class InitPanelController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _rulesPanels;
    
    [Space]
    [Header("Panels")]
    [SerializeField] private RectTransform _chooseLanguagePanel;
    [SerializeField] private RectTransform _startPanel;

    [Space]
    [Header("Configs")]
    [SerializeField] private RectTransform _defaultPanel;
    
    private RectTransform _currentRectTransform;
    private int _currentRuleNumber = 0;

    private void Start()
    {
        _chooseLanguagePanel.gameObject.SetActive(false);
        _startPanel.gameObject.SetActive(false);

        foreach (var rulePanel in _rulesPanels)
        {
            rulePanel.gameObject.SetActive(false);
        }
        
        _defaultPanel.gameObject.SetActive(true);
        _currentRectTransform = _defaultPanel;
        
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.SetLanguage(ELabLanguage.Russia);
    }

    public void OpenRulesPanel()
    {
        SwitchPanel(_rulesPanels[_currentRuleNumber]);
    }

    public void ChooseRussianLanguage()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.SetLanguage(ELabLanguage.Russia);
        SwitchPanel(_startPanel);
    }

    public void ChooseEnglishLanguage()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }
        
        gameManager.SetLanguage(ELabLanguage.English);
        SwitchPanel(_startPanel);
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
