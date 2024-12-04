using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _resultsPanel;

    public void ShowResultPanel()
    {
        _mainPanel.SetActive(false);
        _resultsPanel.SetActive(true);
    }

    public void LoadLab1()
    {
        SceneManager.LoadScene("Lab1");
    }
    
    public void LoadLab2()
    {
        SceneManager.LoadScene("Lab2");
    }

    public void Return()
    {
        _mainPanel.SetActive(true);
        _resultsPanel.SetActive(false);
    }
}
