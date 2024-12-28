using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Machines
{
    public class CleaningSurface : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private WaterDrop _waterDropPrefab;
        [SerializeField] private Transform _waterDropsPool;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private int _targetWaterDropsCount = 5;
        
        private List<WaterDrop> _waterDrops = new List<WaterDrop>();

        private bool _isAllWaterDropsSpawned = false;

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

            WaterDrop waterDrop = Instantiate(_waterDropPrefab, point, Quaternion.identity, _waterDropsPool);
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
            _waterDrops.Clear();
        }
    }
}