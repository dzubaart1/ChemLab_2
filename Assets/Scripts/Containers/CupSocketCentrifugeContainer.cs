using Mechanics;
using UI.Components;
using UnityEngine;

namespace Containers
{
    public class CupSocketCentrifugeContainer : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _button;

        public bool IsClosed()
        {
            return !_button.IsOn;
        }
    }
}