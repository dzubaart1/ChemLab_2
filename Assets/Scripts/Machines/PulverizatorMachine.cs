using System;
using System.Collections.Generic;
using System.Collections;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class PulverizatorMachine : MonoBehaviour
    {
        public event Action WaterDropsEvent;
       
        [CanBeNull] private GameManager _gameManager;
        private XRGrabInteractable _xrGrabInteractable;
        private bool _isAlreadyTriggered = false;
        
        private Ray _ray;
        [SerializeField] private GameObject _rayOrigin;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void Update()
        {
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }
            IXRSelectInteractor interactor = _xrGrabInteractable.interactorsSelecting[0];
            if(interactor is null)
            {
                return;
            }

            ActionBasedController controller = interactor.transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                return;
            }

            if (controller.activateAction.action.triggered)
            {
                _ray = new Ray(_rayOrigin.transform.position, _rayOrigin.transform.up);
                CheckForColliders();
            }
        }

        private void CheckForColliders()
        {
            if (Physics.Raycast(_ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                switch (hit.collider.gameObject.name)
                {
                    case ("Left Controller"):
                    {
                        _gameManager.Game.CompleteTask(new PulverizatorLabActivity(EPulverizatorHits.LeftHandHit));
                        return;
                    }
                    case ("Right Controller"):
                    {
                        _gameManager.Game.CompleteTask(new PulverizatorLabActivity(EPulverizatorHits.RightHandHit));
                        return;
                    }
                    case ("jal"):
                    {
                        _gameManager.Game.CompleteTask(new PulverizatorLabActivity(EPulverizatorHits.PenicilliumHit));
                        return;
                    }
                    case ("Glukoza"):
                    {
                        _gameManager.Game.CompleteTask(new PulverizatorLabActivity(EPulverizatorHits.GlukozaHit));
                        return;
                    }
                    case ("Saharoza"):
                    {
                        _gameManager.Game.CompleteTask(new PulverizatorLabActivity(EPulverizatorHits.SaharozaHit));
                        return;
                    }
                    case ("Laktoza"):
                    {
                        _gameManager.Game.CompleteTask(new PulverizatorLabActivity(EPulverizatorHits.LaktozaHit));
                        return;
                    }
                    case ("WaterDrops"):
                    {
                        WaterDropsEvent?.Invoke();
                        return;
                    }
                }
            }
        }
    }
}