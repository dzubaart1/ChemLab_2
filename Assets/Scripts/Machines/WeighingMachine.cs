using BioEngineerLab.Containers;
using BioEngineerLab.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


namespace BioEngineerLab.Machines
{
    public class WeighingMachine : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private Text _textMesh;

        private void OnTriggerStay(Collider other)
        {
            LabContainer labContainer = other.GetComponent<LabContainer>();

            if (labContainer is null || _socketInteractor.firstInteractableSelected == null)
            {
                return;
            }

            _textMesh.text = labContainer.GetSubstancesWeight() + "g";
        }
    }
}
