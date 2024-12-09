using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Containers
{
    public class ReagentsLabContainer : MonoBehaviour
    {
        [SerializeField] private SOLabSubstanceProperty reagentsLabSubstanceProperty;
        [SerializeField] private LabContainer _labContainer;
        
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
            if (sideEffect is not AddReagentsLabSideEffect addReagentsLabSideEffect)
            {
                return;
            }

            if (addReagentsLabSideEffect.LabSubstanceProperty.Equals(reagentsLabSubstanceProperty.LabSubstanceProperty))
            {
                _labContainer.PutSubstance(new LabSubstance(reagentsLabSubstanceProperty.LabSubstanceProperty, addReagentsLabSideEffect.Weight));
            }
        }
    }
}