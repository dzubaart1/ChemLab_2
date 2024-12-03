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
}
