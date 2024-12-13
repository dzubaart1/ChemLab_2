using System;
using Containers;
using Core;
using Gameplay;
using TMPro;
using UnityEngine;
using Mechanics;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace BioEngineerLab.Machines
{
    public class WeighingMachine : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private TextMeshProUGUI _textMesh;

        private void Update()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                _textMesh.text = "0.0000g";
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                _textMesh.text = "0.0000g";
                return;
            }

            container.ContainerType = EContainer.WeighingContainer;
            _textMesh.text = container.GetSubstancesWeight() == 0 ? "0.0000g" : container.GetSubstancesWeight() + "g";
        }

        private void OnTriggerExit(Collider other)
        {
            LabContainer container = other.GetComponent<LabContainer>();
            container.ContainerType = EContainer.LodochkaContainer;
        }
    }
}
