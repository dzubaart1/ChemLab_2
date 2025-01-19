using Mechanics;
using UnityEngine;

namespace Containers
{
    public class CupSocketLabContainer : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _cupSocket;
        [SerializeField] private bool _isClosedWhenCupIn = true;

        public bool IsClosed()
        {
            if (_isClosedWhenCupIn)
            {
                return _cupSocket.SelectedObject != null;
            }
            else
            {
                return _cupSocket.SelectedObject == null;
            }
        }
    }
}