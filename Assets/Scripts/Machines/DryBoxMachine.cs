using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class DryBoxMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsOn;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _dryButton;
        
        [Space]
        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socketInteractor;
        
        private SavedData _savedData = new SavedData();
        
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
            
            gameManager.CurrentBaseLocalManager.AddSaveableUI(this);
        }
        
        private void OnEnable()
        {
            _dryButton.ClickBtnEvent += OnClickDryBtn;
        }

        private void OnDisable()
        {
            _dryButton.ClickBtnEvent -= OnClickDryBtn;
        }

        private void OnClickDryBtn()
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
            
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container is null)
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer))
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer.LabCraft, container);
        }

        public void SaveUIState()
        {
            _savedData.IsOn = _dryButton.IsOn;
        }

        public void LoadUIState()
        {
            _dryButton.SetIsOn(_savedData.IsOn);
        }
    }
}