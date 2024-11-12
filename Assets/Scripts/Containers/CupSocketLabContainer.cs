using BioEngineerLab.Gameplay;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class CupSocketLabContainer : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _cupSocket;
        [SerializeField] private bool _isNozzleNeed;

        public bool IsClosed()
        {
            if (_cupSocket == null)
            {
                return _isNozzleNeed;
            }

            if (_cupSocket.firstInteractableSelected == null)
            {
                return _isNozzleNeed;
            }
            
            return !_isNozzleNeed;
        }
    }
}