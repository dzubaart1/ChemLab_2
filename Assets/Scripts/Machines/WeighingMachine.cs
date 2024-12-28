using Containers;
using Core;
using TMPro;
using UnityEngine;
using Mechanics;
using UI.Components;

namespace BioEngineerLab.Machines
{
    public class WeighingMachine : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socketInteractor;

        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private ButtonComponent _taraButton;
        
        private float _taraWeight = 0;
        
        private void OnEnable()
        {
            _taraButton.ClickBtnEvent += OnBtnClick;
        }
        
        private void OnDisable()
        {
            _taraButton.ClickBtnEvent -= OnBtnClick;
        }

        private void Update()
        {
            float weight;
            if (_socketInteractor.SelectedObject == null)
            {
                weight = 0 - _taraWeight;
                _textMesh.text = weight.ToString("F4") + "g";
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                weight = 0 - _taraWeight;
                _textMesh.text = weight.ToString("F4") + "g";
                return;
            }

            container.ChangeContainerType(EContainer.WeighingContainer);

            weight = container.GetSubstancesWeight() + container.GetContainerWeight() - _taraWeight;
            _textMesh.text = weight.ToString("F4") + "g";
        }

        private void OnTriggerExit(Collider other)
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
        }

        private void OnBtnClick()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                _taraWeight = 0;
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                _taraWeight = 0;
                return;
            }

            _taraWeight = container.GetContainerWeight();
        }
    }
}
