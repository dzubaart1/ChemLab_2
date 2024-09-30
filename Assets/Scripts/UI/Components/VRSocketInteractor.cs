using System.Collections;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Gameplay
{
    public class VRSocketInteractor : XRSocketInteractor, ISaveable
    {
        private struct SavedData
        {
            public VRGrabInteractable GrabbedObject;
        }

        public SocketType SocketType;
        public bool IsTaskSendable;

        private TasksService _tasksService;
        private SaveService _saveService;

        private SavedData _savedData;
        private bool _isLoadSceneEnter, _isLoadSceneExit;

        private bool _isStartEnter;

        protected override void Awake()
        {
            base.Awake();

            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _isStartEnter = startingSelectedInteractable != null;

            _savedData = new SavedData();
        }

        protected override void Start()
        {
            base.Start();
            
            OnSaveScene();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            Debug.Log($"SELECT ENTER; SOCKET: {gameObject.name}");
            
            if (_isStartEnter)
            {
                Debug.Log($"SELECT ENTER; SKIP START; SOCKET: {gameObject.name}");
                _isStartEnter = !_isStartEnter;
                return;
            }
            
            if (_isLoadSceneEnter)
            {
                Debug.Log($"SELECT ENTER; SKIP LOAD SCENE; SOCKET: {gameObject.name}");
                _isLoadSceneEnter = false;
                return;
            }

            if (IsTaskSendable)
            {
                Debug.Log($"SELECT ENTER; TASK SEND; SOCKET: {gameObject.name}");
                _tasksService.TryCompleteTask(new SocketActivity(SocketType, SocketActivityType.Enter));
            }
        }
        
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (_isLoadSceneExit)
            {
                Debug.Log($"SELECT EXIT; SKIP LOAD SCENE; SOCKET: {gameObject.name}");
                _isLoadSceneExit = false;
                return;
            }
            
            if (IsTaskSendable)
            {
                Debug.Log($"SELECT EXIT; TASK SEND; SOCKET: {gameObject.name}");
                _tasksService.TryCompleteTask(new SocketActivity(SocketType, SocketActivityType.Enter));
            }
        }

        public void OnSaveScene()
        {
            _savedData.GrabbedObject = firstInteractableSelected as VRGrabInteractable;
            if (firstInteractableSelected != null)
            {
                Debug.Log($"SAVED SOCKET {gameObject.name} GRABBED: " + firstInteractableSelected.transform.name);
            }
            else
            {
                Debug.Log($"SAVED SOCKET {gameObject.name} GRABBED: null");
            }
        }

        public void OnLoadScene()
        {
            VRGrabInteractable localGrabInteractable = firstInteractableSelected as VRGrabInteractable;

            if (_savedData.GrabbedObject == null && localGrabInteractable == null)
            {
                Debug.Log($"LOAD; THE SAME NULL; SOCKET: {gameObject.name}; OBJECT null");
                return;
            }

            if (_savedData.GrabbedObject == localGrabInteractable)
            {
                Debug.Log($"LOAD; THE SAME OBJECT; SOCKET: {gameObject.name};" +
                          $" OBJECT {firstInteractableSelected.transform.name}");
                return;
            }

            if (_savedData.GrabbedObject == null && localGrabInteractable != null)
            {
                Debug.Log($"LOAD; SAVE IS NULL, BUT SELECTED IS {firstInteractableSelected.transform.name};" +
                          $" SOCKET: {gameObject.name};");
                
                _isLoadSceneExit = true;
                
                StartCoroutine(ForceDeselect(localGrabInteractable, this));
                return;
            }

            if (_savedData.GrabbedObject != null && localGrabInteractable != null &&
                _savedData.GrabbedObject.isSelected)
            {
                Debug.Log($"LOAD; SAVE IS NOT NULL AND SELECTED IS {firstInteractableSelected.transform.name};" +
                          $" SAVED HELD BY {_savedData.GrabbedObject.firstInteractorSelecting.transform.name};" +
                          $" SOCKET: {gameObject.name};");
                
                _isLoadSceneExit = true;
                _isLoadSceneEnter = true;

                StartCoroutine(ForceDeselect(_savedData.GrabbedObject,
                    _savedData.GrabbedObject.firstInteractorSelecting));
                StartCoroutine(ForceDeselect(localGrabInteractable,
                    this));
                interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
                return;
            }
            
            if (_savedData.GrabbedObject != null && localGrabInteractable != null &&
                !_savedData.GrabbedObject.isSelected)
            {
                Debug.Log($"LOAD; SAVE IS NOT NULL AND SELECTED IS {firstInteractableSelected.transform.name};" +
                          $" SAVED NOT HELD;" +
                          $" SOCKET: {gameObject.name};");
                
                _isLoadSceneExit = true;
                _isLoadSceneEnter = true;

                StartCoroutine(ForceDeselect(localGrabInteractable, this));
                interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
                return;
            }
            
            if (_savedData.GrabbedObject != null && localGrabInteractable == null &&
                !_savedData.GrabbedObject.isSelected)
            {
                Debug.Log($"LOAD; SAVE IS NOT NULL AND SELECTED IS NULL;" +
                          $" SAVED NOT HELD;" +
                          $" SOCKET: {gameObject.name}");
                
                _isLoadSceneEnter = true;
                
                interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
                return;
            }
            
            if (_savedData.GrabbedObject != null && firstInteractableSelected == null &&
                _savedData.GrabbedObject.isSelected)
            {
                Debug.Log($"LOAD; SAVE IS NOT NULL AND SELECTED IS NULL;" +
                          $" SAVED IS HELD BY {_savedData.GrabbedObject.firstInteractorSelecting.transform.name};" +
                          $" SOCKET: {gameObject.name}; OBJECT null");
                
                _isLoadSceneEnter = true;

                StartCoroutine(ForceTransfer(_savedData.GrabbedObject,
                    _savedData.GrabbedObject.firstInteractorSelecting, this));
                return;
            }
        }

        private IEnumerator ForceTransfer(VRGrabInteractable interactable, IXRSelectInteractor from, IXRSelectInteractor to)
        {
            interactionManager.CancelInteractorSelection(from);
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = false;
            yield return null;
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = true;
            interactionManager.SelectEnter(to, interactable);
        }

        private IEnumerator ForceDeselect(VRGrabInteractable interactable, IXRSelectInteractor interactor)
        {
            interactionManager.CancelInteractorSelection(interactor);
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = false;
            yield return null;
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = true;
            
            interactable.LoadPosition();
        }
    }
}