using Core;
using Core.Services;
using SideEffects;
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

        private TasksService _tasksService;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
        }

        private void OnEnable()
        {
            _tasksService.SideEffectActivatedEvent += OnSideEffectActivated;
        }

        private void OnDisable()
        {
            _tasksService.SideEffectActivatedEvent -= OnSideEffectActivated;
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
