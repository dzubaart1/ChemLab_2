using System;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Tasks;
using UnityEngine;

public class Gate : MonoBehaviour, ISaveable
{
    private struct SavedData
    {
        public bool IsOpened;
    }
    
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private Collider _collider;
    
    private const int R_DOOR_XOFFSET_CLOSED = 5;
    private const int R_DOOR_XOFFSET_OPENED = 90;
    private const int L_DOOR_XOFFSET_CLOSED = 5;
    private const int L_DOOR_XOFFSET_OPENED = -90;

    private TasksService _tasksService;
    private SaveService _saveService;

    private bool _isOpened = true;
    private SavedData _savedData;

    private void Awake()
    {
        _saveService = Engine.GetService<SaveService>();
        _saveService.SaveSceneStateEvent += OnSaveScene;
        _saveService.LoadSceneStateEvent += OnLoadScene;

        _tasksService = Engine.GetService<TasksService>();
        _tasksService.TaskUpdatedEvent += OnUpdateTask;
        
        _savedData = new SavedData();
    }

    private void Start()
    {
        ToggleDoors();
        OnSaveScene();
    }

    private void OnDestroy()
    {
        _tasksService.TaskUpdatedEvent -= OnUpdateTask;
        _saveService.LoadSceneStateEvent -= OnLoadScene;
        _saveService.SaveSceneStateEvent -= OnSaveScene;
    }

    private void OnUpdateTask(TaskProperty taskProperty)
    {
        if (taskProperty.ShouldChangeGateOpening & _isOpened != taskProperty.IsOpenGate)
        {
            ToggleDoors();
        }
    }

    public void ToggleDoors()
    {
        _isOpened = !_isOpened;
        
        Vector3 newPos = _leftDoor.transform.localPosition;
        newPos.x = _isOpened ? L_DOOR_XOFFSET_OPENED : L_DOOR_XOFFSET_CLOSED;
        _leftDoor.transform.localPosition = newPos;
        
        newPos = _rightDoor.transform.localPosition;
        newPos.x = _isOpened ? R_DOOR_XOFFSET_OPENED : R_DOOR_XOFFSET_CLOSED;
        _rightDoor.transform.localPosition = newPos;

        _collider.enabled = !_isOpened;
    }

    public void OnSaveScene()
    {
        _savedData.IsOpened = _isOpened;
    }

    public void OnLoadScene()
    {
        if (_tasksService.GetCurrentTask().ShouldChangeGateOpening)
        {
            return;
        }
        
        if (_savedData.IsOpened == _isOpened)
        {
            return;
        }
        
        ToggleDoors();
    }
}
