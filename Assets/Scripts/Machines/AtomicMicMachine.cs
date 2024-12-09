using Core;
using UnityEngine;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class AtomicMicMachine : MonoBehaviour
    {
        [SerializeField] private GameObject _docObject;

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
            
            _gameManager.Game.SideEffectActivatedEvent += OnActivatedSideEffect;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SideEffectActivatedEvent -= OnActivatedSideEffect;
        }
        
        private void OnActivatedSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SpawnDocLabSideEffect spawnDocLabSideEffect)
            {
                return;
            }
            
            spawnDocLabSideEffect = sideEffect as SpawnDocLabSideEffect;

            if (spawnDocLabSideEffect.MachineType == EMachine.AtomicMicMachine)
            {
                _docObject.SetActive(true);
            }
        }
    }
}