using System;
using System.Collections.Generic;
using Activities;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Gameplay;
using Core.Services;
using UnityEngine;
using Mechanics;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;
using BioEngineerLab.Tasks.SideEffects;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class KspectrometrMachine : MonoBehaviour
    {
        private TasksService _tasksService;
        
        [SerializeField] private GameObject _docObject;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
        }

        private void OnEnable()
        {
            _tasksService.SideEffectActivatedEvent += OnActivatedSideEffect;
        }

        private void OnDisable()
        {
            _tasksService.SideEffectActivatedEvent -= OnActivatedSideEffect;
        }

        private void Start()
        {
        }
        
        private void OnActivatedSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SpawnDocLabSideEffect spawnDocLabSideEffect)
            {
                return;
            }
            
            spawnDocLabSideEffect = sideEffect as SpawnDocLabSideEffect;

            if (spawnDocLabSideEffect.MachineType == EMachine.KSpectrometerMachine)
            {
                _docObject.SetActive(true);
            }
        }
    }
}