using System;
using BioEngineerLab.Activities;
using Core;
using Saveables;
using UnityEngine;

namespace Machines
{
    public class Door : MonoBehaviour, ISaveableDoor
    {
        private class SavedData
        {
            public bool IsOpen;
        }

        public event Action DoorOpenedEvent;
        public event Action DoorClosedEvent;
        
        [Header("Refs")]
        [SerializeField] private Transform _door;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private Quaternion _closed;
        [SerializeField] private Quaternion _opened;
        [SerializeField] private EDoor _doorType;
        [SerializeField] private bool _isOpenTaskSendable  = true;
        [SerializeField] private bool _isCloseTaskSendable  = true;
        
        private bool _isOpen = false;
        private SavedData _savedData = new SavedData();
        
        private void Update()
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
            
            if (!IsRotationEqual(_closed, 0.01f) && !_isOpen)
            {
                _isOpen = true;
                if (_isOpenTaskSendable)
                {
                    gameManager.CurrentBaseLocalManager.OnActivityComplete(new DoorLabActivity(_doorType, EDoorActivity.Open));
                }
                
                DoorOpenedEvent?.Invoke();
            }
            
            else if (IsRotationEqual(_closed, 0.01f) && _isOpen)
            {
                _isOpen = false;
                if (_isCloseTaskSendable)
                {
                    gameManager.CurrentBaseLocalManager.OnActivityComplete(new DoorLabActivity(_doorType, EDoorActivity.Closed));
                }
                
                DoorClosedEvent?.Invoke();
            }
        }

        private bool IsRotationEqual(Quaternion q, float accuracy)
        {
            bool result = true;
            
            result &= (Math.Abs(_door.transform.rotation.x - q.x) < accuracy);
            
            result &= (Math.Abs(_door.transform.rotation.y - q.y) < accuracy);
            
            result &= (Math.Abs(_door.transform.rotation.z - q.z) < accuracy);
            
            return result;
        }

        public void SaveDoorState()
        {
            _savedData.IsOpen = _isOpen;
        }

        public void LoadDoorState()
        {
            if (_savedData.IsOpen)
            {
                _isOpen = true;
                _door.transform.rotation = _opened;
            }
            else
            {
                _isOpen = false;
                _door.transform.rotation = _closed;
                Rigidbody rb = _door.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
            }
        }
    }
}