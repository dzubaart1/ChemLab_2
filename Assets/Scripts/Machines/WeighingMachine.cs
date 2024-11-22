using System;
using BioEngineerLab.Containers;
using BioEngineerLab.Gameplay;
using TMPro;
using UnityEngine;
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
                _textMesh.text = "0g";
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                _textMesh.text = "0g";
                return;
            }

            _textMesh.text = container.GetSubstancesWeight() + "g";
        }
    }
}
