using System;
using Core;
using JetBrains.Annotations;
using Machines;
using Mechanics;
using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private VRGrabInteractable _vrGrabInteractable;
    [SerializeField] private Rigidbody _doorRb;
    [SerializeField] [CanBeNull] private Door _door;
    private Rigidbody _rb;
    private Player _player;
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
}
