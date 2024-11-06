using BioEngineerLab.Containers;
using BioEngineerLab.Gameplay;
using UnityEngine;
using UnityEngine.UI;


namespace BioEngineerLab.Machines
{
    public class WeighingMachine : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private Text _textMesh;

        private void OnTriggerStay(Collider other)
        {
            LabContainer labContainer = other.GetComponent<LabContainer>();

            if (labContainer == null | _socketInteractor.SelectedObject == null)
            {
                return;
            }

            _textMesh.text = labContainer.GetSubstancesWeight() + "g";
        }
    }
}
