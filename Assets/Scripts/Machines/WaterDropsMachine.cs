using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

public class WaterDropsMachine : MonoBehaviour
{
    [SerializeField] private int _waterDropsCount;
    [SerializeField] private GameObject _waterDropsPrefab;
    
    private bool _waterDropsActive = false;

    [CanBeNull] private GameManager _gameManager;
    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void MinusCount()
    {
        _waterDropsCount--;
        if (_waterDropsCount == 0)
        {
            _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.WaterDropsMachine));
            _waterDropsCount--;
            _waterDropsActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_waterDropsActive)
        {
            return;
        }
        if (other.CompareTag("Pulverizator") )
        {
            _waterDropsPrefab.SetActive(true);
            _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnStart,
                EMachine.WaterDropsMachine));
            _waterDropsActive = true;
        }
    }
}
