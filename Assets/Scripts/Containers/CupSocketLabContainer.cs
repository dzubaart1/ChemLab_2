using BioEngineerLab.Gameplay;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class CupSocketLabContainer : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _cupSocket;

        public bool IsClosed()
        {
            if (_cupSocket == null)
            {
                return false;
            }

            if (_cupSocket.firstInteractableSelected == null)
            {
                return false;
            }
            
            return true;
        }
    }
}