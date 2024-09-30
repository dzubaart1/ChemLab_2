using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Configurations;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.UI.Components;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Syringe : MonoBehaviour
{
    [SerializeField] private SyringeCupMove _syringeCupMove;
    [SerializeField] private VRGrabInteractable _vrGrabInteractable;
    [SerializeField] private Animator _dropAnimator;
    [SerializeField] private ButtonComponent _syringeButton;
    [SerializeField] private Container _container;

    private TasksService _tasksService;

    private DropAnimationService _dropAnimationService;
    
    private void Awake()
    {
        _dropAnimationService = Engine.GetService<DropAnimationService>();
        _dropAnimationService.StartAnimationEvent += OnStartAnimation;
        _tasksService = Engine.GetService<TasksService>();

        _syringeButton.OnClickButton += OnSyringeButtonClick;
    }

    private void OnDestroy()
    {
        _dropAnimationService.StartAnimationEvent -= OnStartAnimation;
        _syringeButton.OnClickButton -= OnSyringeButtonClick;
    }

    private void OnSyringeButtonClick()
    {
        _syringeCupMove.OnClickSyringeButton();
        
        IXRSelectInteractor interactor = _vrGrabInteractable.firstInteractorSelecting;

        if (interactor == null || interactor.transform.GetComponent<VRSocketInteractor>() == null)
        {
            return;
        }
        
        _dropAnimationService.PlayAnimation(_container.PeekLastSubstance().SubstanceProperty.SubstanceName);
    }

    private void OnStartAnimation(DropAnimationConfiguration.DropAnimationSubstance animationSubstance)
    {
        //_dropAnimator.Play(animationSubstance.AnimationName, 0, animationSubstance.AnimationDuration);
    }
}
