using Core;
using Core.Services;
using SideEffects;
using Substances;
using UnityEngine;
using UnityEngine.Serialization;

namespace Containers
{
    [RequireComponent(typeof(LabContainer))]
    public class ReagentsLabContainer : MonoBehaviour
    {
        [FormerlySerializedAs("_reagentsSubstanceProperty")] [SerializeField] private SOLabSubstanceProperty reagentsLabSubstanceProperty;
        
        private TasksService _tasksService;

        private LabContainer _labContainer;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();

            _labContainer = GetComponent<LabContainer>();
        }

        private void OnEnable()
        {
            _tasksService.SideEffectActivatedEvent += OnActivatedSideEffect;
        }

        private void OnDisable()
        {
            _tasksService.SideEffectActivatedEvent -= OnActivatedSideEffect;
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