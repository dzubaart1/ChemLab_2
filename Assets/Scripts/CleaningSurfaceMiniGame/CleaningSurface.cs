using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Activities;
using Core;
using Saveables;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Machines
{
    public class CleaningSurface : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public List<WaterDrop> WaterDrops;
            public bool IsAllWaterDropsSpawned;
        }
        
        [Header("Refs")]
        [SerializeField] private WaterDrop _waterDropPrefab;
        [SerializeField] private Transform _waterDropsPool;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private int _targetWaterDropsCount = 5;

        private SavedData _savedData = new SavedData();
        
        private List<WaterDrop> _waterDrops = new List<WaterDrop>();
        private bool _isAllWaterDropsSpawned = false;

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
            
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
        }

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
            
            if (!_isAllWaterDropsSpawned)
            {
                return;
            }

            if (_waterDrops.All(waterDrop => !waterDrop.gameObject.activeSelf))
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.WaterDropsMachine));
                Reset();
            }
        }

        public void OnPulverizatorHit(Vector3 point)
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
            
            if (_waterDrops.Count >= _targetWaterDropsCount)
            {
                return;
            }

            float y = UnityEngine.Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(0, y, 0);

            WaterDrop waterDrop = Instantiate(_waterDropPrefab, point, rotation, _waterDropsPool);
            _waterDrops.Add(waterDrop);

            if (_waterDrops.Count == _targetWaterDropsCount)
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnStart, EMachine.WaterDropsMachine));
                _isAllWaterDropsSpawned = true;
            }
        }

        private void Reset()
        {
            _isAllWaterDropsSpawned = false;
            foreach (WaterDrop waterDrop in _waterDrops)
            {
                Destroy(waterDrop.gameObject);
            }
            _waterDrops.Clear();
        }

        public void Save()
        {
            _savedData.WaterDrops = _waterDrops;
            _savedData.IsAllWaterDropsSpawned = _isAllWaterDropsSpawned;
        }

        public void Load()
        {
            _isAllWaterDropsSpawned = _savedData.IsAllWaterDropsSpawned;

            if (_isAllWaterDropsSpawned)
            {
                _waterDrops.Clear();
                foreach (WaterDrop waterDrop in _waterDrops)
                {
                    Destroy(waterDrop.gameObject);
                }
                foreach (WaterDrop waterDrop in _savedData.WaterDrops)
                {
                    _waterDrops.Add(waterDrop);
                }
            }
            else
            {
                foreach (WaterDrop waterDrop in _waterDrops)
                {
                    Destroy(waterDrop.gameObject);
                }
                _waterDrops.Clear();
            }
        }
    }
}