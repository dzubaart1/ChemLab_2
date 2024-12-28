using System.Collections;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using JetBrains.Annotations;
using Crafting;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class Spatula : MonoBehaviour, ISaveableOther
    {
        private struct SavedData
        {
            public bool IsShellVisible;
        }
        
        [Header("Refs")]
        [SerializeField] private VRGrabInteractable _vrGrabInteractable;
        [SerializeField] private MeshRenderer _shell;
        
        [Space]
        [Header("UIs")]
        [SerializeField] private ButtonComponent _shellToggleVisibleButton;
        
        private SavedData _savedData = new SavedData();
        
        private bool _isAlreadyTriggered = false;
        private bool _isShellVisible = true;
        
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
        }


        private void OnEnable()
        {
            _shellToggleVisibleButton.ClickBtnEvent += OnShellToggleVisibleButtonClick;
        }
        
        private void OnDisable()
        {
            _shellToggleVisibleButton.ClickBtnEvent -= OnShellToggleVisibleButtonClick;
        }

        private void OnShellToggleVisibleButtonClick()
        {
            _isShellVisible = false;
            _shell.enabled = false;
        }

        private void OnTriggerStay(Collider other)
        {
            GameManager gameManager = GameManager.Instance;
            if(gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            if (_isShellVisible)
            {
                return;
            }
            
            if (_vrGrabInteractable.interactorsSelecting.Count == 0)
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

            IXRSelectInteractor interactor = _vrGrabInteractable.interactorsSelecting[0];
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
                if (CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), labContainer.GetSubstanceProperties() ,ECraft.HeatStir ,out SOLabCraft labCraft))
                {
                    CraftTools.ApplyCraft(labCraft.LabCraft, labContainer);
                    gameManager.CurrentBaseLocalManager.OnActivityComplete(new CraftSubstanceLabActivity(labContainer.ContainerType, labCraft.LabCraft));
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

        public void Save()
        {
            _savedData.IsShellVisible = _isShellVisible;
        }

        public void Load()
        {
            _isShellVisible = _savedData.IsShellVisible;
            _shell.enabled = _isShellVisible;
        }
    }
}