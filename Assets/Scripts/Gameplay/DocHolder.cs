using BioEngineerLab.Tasks.SideEffects;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay
{
    public class DocHolder : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _spawnPoint;
    
        [Space]
        [Header("Configs")]
        [SerializeField] private EMachine _machine;
        [SerializeField] private GameObject _docPrefab;

        [CanBeNull] private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SideEffectActivatedEvent += OnSideEffectActivated;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SideEffectActivatedEvent -= OnSideEffectActivated;
        }

        private void OnSideEffectActivated(LabSideEffect labSideEffect)
        {
            if (labSideEffect is not SpawnDocLabSideEffect spawnDocLabSideEffect)
            {
                return;
            }

            if (spawnDocLabSideEffect.MachineType != _machine)
            {
                return;
            }

            Instantiate(_docPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}
