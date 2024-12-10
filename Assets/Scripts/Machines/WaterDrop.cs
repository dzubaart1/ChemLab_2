using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private WaterDropsMachine _waterDropsMachine;

    private void Awake()
    {
        _waterDropsMachine = GetComponentInParent<WaterDropsMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Towel"))
        {
            _waterDropsMachine.MinusCount();
            gameObject.SetActive(false);
        }
    }
}
