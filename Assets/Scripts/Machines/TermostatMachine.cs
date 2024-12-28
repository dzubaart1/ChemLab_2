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
    public class TermostatMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsPower;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        
        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socketInteractor1;
        [SerializeField] private VRSocketInteractor _socketInteractor2;
        [SerializeField] private Door _door;
        
        private SavedData _savedData = new SavedData();
        
        private void OnEnable()
        {
            _door.DoorClosedEvent += OnDoorClosed;
        }

        private void OnDisable()
        {
            _door.DoorClosedEvent -= OnDoorClosed;
        }

        private void OnDoorClosed()
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
            
            if (!_powerButton.IsOn)
            {
                return;
            }
            
            if (_socketInteractor1.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container1 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();

            if (container1 is null)
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container1.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer1))
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer1.LabCraft, container1);
            
            if (_socketInteractor2.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container2 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();

            if (container2 is null)
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container2.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer2))
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer2.LabCraft, container2);
        }
        
        public void SaveUIState()
        {
            _savedData.IsPower = _powerButton.IsOn;
        }

        public void LoadUIState()
        {
            _powerButton.SetIsOn(_savedData.IsPower);
        }
    }
}