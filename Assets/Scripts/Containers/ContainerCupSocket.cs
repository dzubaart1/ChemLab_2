using BioEngineerLab.Gameplay;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class ContainerCupSocket : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _cupSocket;

        public bool IsClosed()
        {
            return _cupSocket == null || _cupSocket.firstInteractableSelected != null;
        }
    }
}