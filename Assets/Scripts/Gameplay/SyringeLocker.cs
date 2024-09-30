using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Gameplay
{
    public class SyringeLocker : MonoBehaviour
    {
        [SerializeField] private List<VRSocketInteractor> _sockets;

        private TasksService _tasksService;
        
        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _tasksService.TaskUpdatedEvent += OnUpdateTask;
            
            foreach (var socket in _sockets)
            {
                socket.selectEntered.AddListener(OnSelectEnter);
                socket.selectExited.AddListener(OnSelectExit);
            }
        }

        private void OnDestroy()
        {
            _tasksService.TaskUpdatedEvent -= OnUpdateTask;
        }

        private void OnUpdateTask(TaskProperty taskProperty)
        {
            if (!taskProperty.UnlockSyringe)
            {
                return;
            }
            
            foreach (var socket in _sockets)
            {
                ToggleColliders(socket.firstInteractableSelected, true);
            }
        }

        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            ToggleColliders(args.interactableObject, false);
        }
        
        private void OnSelectExit(SelectExitEventArgs args)
        {
            ToggleColliders(args.interactableObject, true);
        }

        private void ToggleColliders(IXRSelectInteractable interactable, bool isEnabled)
        {
            if (interactable == null)
            {
                return;
            }
            
            var colliders = interactable.transform.GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                collider.enabled = isEnabled;
            }
        }
    }
}