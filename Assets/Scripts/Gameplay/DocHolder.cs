using BioEngineerLab.Tasks.SideEffects;
using Core;
using UnityEngine;

namespace Gameplay
{
    public class DocHolder : MonoBehaviour, ISideEffectActivator
    {
        [Header("Refs")]
        [SerializeField] private Transform _spawnPoint;
    
        [Space]
        [Header("Configs")]
        [SerializeField] private EMachine _machine;
        [SerializeField] private GameObject _docPrefab;

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
            
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
        }

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SpawnDocLabSideEffect spawnDocLabSideEffect)
            {
                return;
            }

            if (spawnDocLabSideEffect.MachineType != _machine)
            {
                return;
            }

            Instantiate(_docPrefab, _spawnPoint.position, _spawnPoint.rotation);
        }
    }
}
