using BioEngineerLab.Tasks.SideEffects;
using Containers;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

namespace BioEngineerLab.Machines
{
    public class DozatorMachine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
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
