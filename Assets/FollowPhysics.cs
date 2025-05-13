using System;
using Core;
using JetBrains.Annotations;
using Machines;
using Mechanics;
using UnityEngine;
using Saveables;

public class FollowPhysics : MonoBehaviour, ISaveableOther
{
    private class SavedData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    
    [SerializeField] private Rigidbody _target;
    [SerializeField] private VRGrabInteractable _vrGrabInteractable;
    [SerializeField] private Rigidbody _doorRb;
    [SerializeField] [CanBeNull] private Door _door;
    private Rigidbody _rb;
    private Player _player;
    private SavedData _savedData = new SavedData();

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        if (gameManager.CurrentBaseLocalManager == null)
        {
            return;
        }
        
        gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
        _player = gameManager.PlayerSpawner.Player;
    }
    void FixedUpdate()
    {
        _rb.MovePosition(_target.position);
    }

    private void OnEnable()
    {
        _vrGrabInteractable.UngrabbedEvent += OnUngrab;
        /*if (_door != null)
        {
            _door.DoorClosedEvent += OnDoorClosed;
        }*/
    }

    private void OnDisable()
    {
        _vrGrabInteractable.UngrabbedEvent -= OnUngrab;
        /*if (_door != null)
        {
            _door.DoorClosedEvent -= OnDoorClosed;
        }*/
    }

    private void OnUngrab()
    {
        _target.position = _rb.position;
        _target.rotation = _rb.rotation;
        
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
        
        _doorRb.angularVelocity = Vector3.zero;
        _doorRb.velocity = Vector3.zero;
    }

    private void OnDoorClosed()
    {
        _player.ReleaseAllGrabbables();
    }

    public void ResetHandler()
    {
        _target.position = _rb.position;
        _target.rotation = _rb.rotation;
        
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
    }
    
    public void Save()
    {
        _savedData.Position = _target.position;
        _savedData.Rotation = _target.rotation;
    }

    public void Load()
    {
        //Rigidbody rb = _target.gameObject.GetComponent<Rigidbody>();
        _target.position = _savedData.Position;
        _target.rotation = _savedData.Rotation;
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
    }
}
