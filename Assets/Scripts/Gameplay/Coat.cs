using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(VRGrabInteractable))]
public class Coat : MonoBehaviour, ISaveable
{
    private struct SavedData
    {
        public bool IsVisible;
    }
    
    [SerializeField] private MeshRenderer _renderer;

    private SaveService _saveService;
    private TasksService _tasksService;

    private VRGrabInteractable _vrGrabInteractable;
    private SavedData _savedData;

    private void Awake()
    {
        _tasksService = Engine.GetService<TasksService>();
        
        _saveService = Engine.GetService<SaveService>();
        _saveService.SaveSceneStateEvent += OnSaveScene;
        _saveService.LoadSceneStateEvent += OnLoadScene;
        
        _vrGrabInteractable = GetComponent<VRGrabInteractable>();
        _vrGrabInteractable.selectEntered.AddListener(OnSelectEntered);

        _savedData = new SavedData();
    }

    private void Start()
    {
        OnSaveScene();
    }

    private void OnDestroy()
    {
        _saveService.SaveSceneStateEvent -= OnSaveScene;
        _saveService.LoadSceneStateEvent -= OnLoadScene;
        
        _vrGrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }

    public void OnSaveScene()
    {
        _savedData.IsVisible = _renderer.enabled;
    }

    public void OnLoadScene()
    {
        _renderer.enabled = _savedData.IsVisible;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.GetComponent<VRSocketInteractor>())
        {
            return;    
        }

        _renderer.enabled = false;
        _vrGrabInteractable.interactionManager.SelectExit(args.interactorObject, _vrGrabInteractable);
        _tasksService.TryCompleteTask(new MachineActivity(MachineActivityType.OnStart, MachineType.CoatMachine));
    }
}
