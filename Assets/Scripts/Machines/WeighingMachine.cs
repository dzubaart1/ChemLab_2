using Containers;
using Mechanics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Machines
{
    public class WeighingMachine : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [FormerlySerializedAs("_textMesh")] [SerializeField] private Text _weightText;

        private void Update()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                _weightText.text = "0g";
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container == null)
            {
                _weightText.text = "0g";
                return;
            }

            _weightText.text = container.GetSubstancesWeight() + "g";
        }
    }
}
