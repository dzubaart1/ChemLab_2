using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _lab1Panel;
    [SerializeField] private GameObject _lab2Panel;
    [SerializeField] private GameObject [] _rulesPanels;

    private int _ruleNumber = 0;

    public void OpenLab1Panel()
    {
        _startPanel.SetActive(false);
        _lab1Panel.SetActive(true);
    }
    
    public void OpenLab2Panel()
    {
        _startPanel.SetActive(false);
        _lab2Panel.SetActive(true);
    }

    public void OpenRulesPanel()
    {
        _startPanel.SetActive(false);
        _rulesPanels[_ruleNumber].SetActive(true);
    }

    public void BeginLab1()
    {
        SceneManager.LoadScene("Lab1");
    }
    
    public void BeginLab2()
    {
        SceneManager.LoadScene("Lab2");
    }

    public void Return()
    {
        _startPanel.SetActive(true);
        _lab1Panel.SetActive(false);
        _lab2Panel.SetActive(false);
        _rulesPanels[_ruleNumber].SetActive(false);
    }

    public void NextRule()
    {
        if (_ruleNumber == _rulesPanels.Length - 1)
            return;
        _rulesPanels[_ruleNumber++].SetActive(false);
        _rulesPanels[_ruleNumber].SetActive(true);
    }
    
    public void PrevRule()
    {
        if (_ruleNumber == 0)
            return;
        _rulesPanels[_ruleNumber--].SetActive(false);
        _rulesPanels[_ruleNumber].SetActive(true);
    }
}
