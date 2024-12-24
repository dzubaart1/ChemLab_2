using System;
using System.Collections.Generic;
using System.Collections;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using JetBrains.Annotations;
using Crafting;
using UI.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class ShpatelMachine : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool IsPacked;
        }
        [CanBeNull] private GameManager _gameManager;
        private XRGrabInteractable _xrGrabInteractable;
        private bool _isAlreadyTriggered = false;

        private bool _isPacked = true;
        private SavedData _savedData = new SavedData();

        [SerializeField] private ButtonComponent _button;
        [SerializeField] private GameObject _packPrefab;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            
            _button.ClickBtnEvent += OnButtonClick;
        }
        
        private void OnDisable()
        {
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            
            _button.ClickBtnEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            _isPacked = false;
            _packPrefab.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            if (_isPacked)
            {
                return;
            }
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }

            CupSocketLabContainer targetCupSocketLabContainerCup = other.GetComponent<CupSocketLabContainer>();
            if(targetCupSocketLabContainerCup is null)
            {
                return;
            }

            if (targetCupSocketLabContainerCup.IsClosed())
            {
                return;
            }

            IXRSelectInteractor interactor = _xrGrabInteractable.interactorsSelecting[0];
            if(interactor is null)
            {
                return;
            }

            ActionBasedController controller = interactor.transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                return;
            }
            
            LabContainer labContainer = other.GetComponent<LabContainer>();

            if (!_isAlreadyTriggered & controller.activateAction.action.triggered)
            {
                if (CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, labContainer.GetSubstanceProperties() ,ECraft.HeatStir ,out SOLabCraft labCraft))
                {
                    CraftTools.ApplyCraft(labCraft.LabCraft, labContainer);
                    _gameManager.CompleteTask(new CraftSubstanceLabActivity(labContainer.ContainerType, labCraft.LabCraft));
                }
                StartCoroutine(StartDelayBetweenActivated());
            }
        }
        private IEnumerator StartDelayBetweenActivated()
        {
            _isAlreadyTriggered = true;
            yield return new WaitForSeconds(0.5f);
            _isAlreadyTriggered = false;
        }

        public void OnSaveScene()
        {
            _savedData.IsPacked = _isPacked;
        }

        public void OnLoadScene()
        {
            _isPacked = _savedData.IsPacked;
            _packPrefab.SetActive(_isPacked);
        }
    }
}