using Containers;
using Core;
using TMPro;
using UnityEngine;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine.Serialization;

namespace BioEngineerLab.Machines
{
    public class WeighingMachine : MonoBehaviour, ISaveableOther
    {
        private struct SavedData
        {
            public float TaraWeight;
        }
        
        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socketInteractor;

        [FormerlySerializedAs("_textMesh")]
        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _weightText;
        [SerializeField] private ButtonComponent _taraButton;

        private SavedData _savedData = new SavedData();
        
        private float _taraWeight = 0;
        
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
                _taraWeight = 0f;
                UpdateWeightText(0f);
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

        public void Save()
        {
            _savedData.TaraWeight = _taraWeight;
        }

        public void Load()
        {
            _taraWeight = _savedData.TaraWeight;
        }
    }
}
