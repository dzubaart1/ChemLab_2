using System.Collections.Generic;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;

public class LabSplitPanelController : MonoBehaviour
{
    [Header("Panels")] 
    [SerializeField] private RectTransform _startPanel;
    [SerializeField] private RectTransform _lab1Panel;
    [SerializeField] private RectTransform _lab2Panel;

    [CanBeNull] private GameManager _gameManager;
    
    private RectTransform _currentRectTransform;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        _currentRectTransform = _startPanel;
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
    
    public void BeginLab3()
    {
        if (_gameManager == null)
        {
            return;
        }
        
        _gameManager.StartGame(ELab.Lab3);
        _gameManager.LoadScene(Game.LAB_3_SCENE_NAME);
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
