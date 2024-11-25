using Containers;
using Core;
using Core.Services;
using SideEffects;
using UnityEngine;
using UnityEngine.UI;

namespace Machines
{
    public class DozatorMachine : MonoBehaviour
    {
        [SerializeField] private Text _text;
        
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
            if (sideEffect is not SetDozatorVolumeLabSideEffect setDozatorVolumeLabSideEffect)
            {
                return;
            }

            if (setDozatorVolumeLabSideEffect.DozatorVolume >= 1)
            {
                _text.text = setDozatorVolumeLabSideEffect.DozatorVolume + ".00";
            }
            else
            {
                _text.text = setDozatorVolumeLabSideEffect.DozatorVolume + "";
            }
            
            _labContainer.ChangeMaxVolume(setDozatorVolumeLabSideEffect.DozatorVolume);
        }
    }
}
