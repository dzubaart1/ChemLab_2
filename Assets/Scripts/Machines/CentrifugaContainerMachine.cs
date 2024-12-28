using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class CentrifugaContainerMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsOpen;
        }
        
        [SerializeField] private ButtonComponent _openButton;
        [SerializeField] private Animator _animator;

        [SerializeField] private string _openState = "Open";
        [SerializeField] private string _closeState = "Close";
        
        private SavedData _savedData = new SavedData();
        
        private void OnEnable()
        {
            _openButton.ClickBtnEvent += OnClickOpenButton;
        }

        private void OnDisable()
        {
            _openButton.ClickBtnEvent -= OnClickOpenButton;
        }

        private void OnClickOpenButton()
        {
            _animator.Play(_openButton.IsOn ? _openState : _closeState);
        }

        public void SaveUIState()
        {
            _savedData.IsOpen = _openButton.IsOn;
        }

        public void LoadUIState()
        {
            _openButton.SetIsOn(_savedData.IsOpen);
            _animator.Play(_openButton.IsOn ? _openState : _closeState);
        }
    }
}