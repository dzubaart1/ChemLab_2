using System.Collections.Generic;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using Mechanics;
using UnityEngine;

namespace Constructors
{
    public class Constructor : MonoBehaviour, ISideEffectActivator
    {
        [SerializeField] private List<VRSocketInteractor> _sockets;
        
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
        }

        
        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not ConstructorSideEffect constructorSideEffect)
            {
                return;
            }

            foreach (var socket in _sockets)
            {
                if (socket.SocketType == constructorSideEffect.SocketType)
                {
                    if (constructorSideEffect.IsLock)
                    {
                        socket.Lock();
                    }
                    else
                    {
                        socket.UnLock();
                    }
                }
            }
        }
    }
}