using Containers;
using Core;
using TMPro;
using UnityEngine;
using Mechanics;
using UI.Components;
using UnityEngine.Serialization;

namespace BioEngineerLab.Machines
{
    public class WeighingMachine : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socketInteractor;

        [FormerlySerializedAs("_textMesh")]
        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _weightText;
        [SerializeField] private ButtonComponent _taraButton;
        
        private float _taraWeight = 0;
        
        private void OnEnable()
        {
            _socketInteractor.ExitedTransformEvent += OnExited;
            _socketInteractor.EnteredTransformEvent += OnEntered;
            _taraButton.ClickBtnEvent += OnBtnClick;
        }
        
        private void OnDisable()
        {
            _socketInteractor.ExitedTransformEvent -= OnExited;
            _socketInteractor.EnteredTransformEvent -= OnEntered;
            _taraButton.ClickBtnEvent -= OnBtnClick;
        }

        private void Update()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                return;
            }
                
            UpdateWeightText(container.GetSubstancesWeight() + container.GetContainerWeight() - _taraWeight);
        }

        private void OnExited(Transform other)
        {
            LabContainer container = other.GetComponent<LabContainer>();
            
            if (container == null)
            {
                return;
            }
            
            if (container.ContainerType == EContainer.WeighingContainer)
            {
                container.ChangeContainerType(EContainer.LodochkaContainer);
            }
            
            _taraWeight = 0f;
            UpdateWeightText(0f);            
        }

        private void OnEntered(Transform other)
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                return;
            }

            container.ChangeContainerType(EContainer.WeighingContainer);
            UpdateWeightText(container.GetSubstancesWeight() + container.GetContainerWeight());
        }

        private void OnBtnClick()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                return;
            }

            _taraWeight = container.GetContainerWeight() + container.GetSubstancesWeight();
        }

        private void UpdateWeightText(float weight)
        {
            _weightText.text = weight.ToString("F4") + "g";
        }
    }
}
