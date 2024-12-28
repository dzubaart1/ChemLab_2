using Core;
using Core.Services;
using Containers;
using Crafting;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class ShakerMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsShaking;
        }
        
        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _rpmButton;

        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socket1;
        [SerializeField] private VRSocketInteractor _socket2;   
        [SerializeField] private VRSocketInteractor _socket3;
        [SerializeField] private Animator _animator;
        
        private SavedData _savedData = new SavedData();
        
        private bool _isShaking = false;

        private void OnEnable()
        {
            _rpmButton.ClickBtnEvent += OnRpmButtonClick;
        }

        private void OnDisable()
        {
            _rpmButton.ClickBtnEvent -= OnRpmButtonClick;
        }
        
        private void OnRpmButtonClick()
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
            
            _animator.enabled = _rpmButton.IsOn;
            _isShaking = _rpmButton.IsOn;

            if (_isShaking)
            {
                return;
            }
            
            if (_socket1.SelectedObject == null || _socket2.SelectedObject == null || _socket3.SelectedObject == null)
            {
                return;
            }
                    
            LabContainer container1 = _socket1.SelectedObject.transform.GetComponent<LabContainer>();
            LabContainer container2 = _socket2.SelectedObject.transform.GetComponent<LabContainer>();
            LabContainer container3 = _socket3.SelectedObject.transform.GetComponent<LabContainer>();
                    
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container1.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft1))
            {
                return;
            }
            CraftTools.ApplyCraft(labCraft1.LabCraft, container1);
                    
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container2.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft2))
            {
                return;
            }
            CraftTools.ApplyCraft(labCraft2.LabCraft, container2);
                    
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container3.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft3))
            {
                return;
            }
            CraftTools.ApplyCraft(labCraft3.LabCraft, container3);
        }
        
        public void SaveUIState()
        {
            _savedData.IsShaking = _isShaking;
        }

        public void LoadUIState()
        {
            _animator.enabled = _savedData.IsShaking;
            _isShaking = _savedData.IsShaking;
        }
    }
}