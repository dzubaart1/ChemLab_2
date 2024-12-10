using System.Collections;
using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using Core;
using Core.Services;
using Crafting;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Containers
{
    [RequireComponent(typeof(Collider), typeof(VRGrabInteractable), typeof(CupSocketLabContainer))]
    public class SubstanceTransferLabContainer : MonoBehaviour
    {
        private const float DELAY_TRIGGERED = 0.5f;

        [CanBeNull] private GameManager _gameManager;
        
        private XRGrabInteractable _xrGrabInteractable;
        private LabContainer _labContainer;
        private CupSocketLabContainer _cupSocketLabContainer;
        
        private bool _isAlreadyTriggered;
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
            if (_gameManager == null)
            {
                return;
            }
            
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
            _labContainer = GetComponent<LabContainer>();
            _cupSocketLabContainer = GetComponent<CupSocketLabContainer>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }
            if (_cupSocketLabContainer.IsClosed())
            {
                return;
            }
            
            SubstanceTransferLabContainer targetSubstanceTransferLabContainer = other.GetComponent<SubstanceTransferLabContainer>();
            if(targetSubstanceTransferLabContainer is null)
            {
                return;
            }

            CupSocketLabContainer targetCupSocketLabContainerCup = other.GetComponent<CupSocketLabContainer>();
            if(targetCupSocketLabContainerCup is null)
            {
                return;
            }

            if (targetCupSocketLabContainerCup.IsClosed())
            {
                return;
            }

            IXRSelectInteractor interactor = _xrGrabInteractable.interactorsSelecting[0];
            if(interactor is null)
            {
                return;
            }

            ActionBasedController controller = interactor.transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                return;
            }

            if (!_isAlreadyTriggered & controller.activateAction.action.triggered)
            {
                TryTransfer(_labContainer, targetSubstanceTransferLabContainer._labContainer);
                StartCoroutine(StartDelayBetweenActivated());
            }
        }

        private void TryTransfer(LabContainer fromLabContainer, LabContainer toLabContainer)
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() == 0 &
                toLabContainer.GetSubstancesCount() != 0)
            {
                if (CraftTools.TryAdd(toLabContainer, fromLabContainer, out LabSubstance transferSubstance))
                {
                    _gameManager.Game.CompleteTask(new AddSubstanceLabActivity(
                        toLabContainer.ContainerType,
                        fromLabContainer.ContainerType,
                        transferSubstance.SubstanceProperty));   
                }
                return;
            }

            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() != 0 &
                toLabContainer.GetSubstancesCount() != 0)
            {
                if (CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, fromLabContainer.GetSubstanceProperties().Concat(toLabContainer.GetSubstanceProperties()).ToList(),ECraft.Mix ,out SOLabCraft targetCraft))
                {
                    _gameManager.Game.CompleteTask(new CraftSubstanceLabActivity(toLabContainer.ContainerType, targetCraft.LabCraft));
                }
                return;
            }

            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() != 0 &
                toLabContainer.GetSubstancesCount() == 0)
            {
                if (CraftTools.TryAdd(fromLabContainer, toLabContainer, out LabSubstance transferSubstance))
                {
                    _gameManager.Game.CompleteTask(new AddSubstanceLabActivity(
                        fromLabContainer.ContainerType,
                        toLabContainer.ContainerType,
                        transferSubstance.SubstanceProperty));   
                }
                return;
            }
            
            if (fromLabContainer.GetSubstancesCount() == 0)
            {
                return;
            }

            if (toLabContainer.GetSubstancesCount() != 0)
            {
                if (CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, fromLabContainer.GetSubstanceProperties().Concat(toLabContainer.GetSubstanceProperties()).ToList(),ECraft.Mix ,out SOLabCraft targetCraft))
                {
                    CraftTools.Mix(targetCraft.LabCraft, fromLabContainer, toLabContainer);
                    _gameManager.Game.CompleteTask(new CraftSubstanceLabActivity(toLabContainer.ContainerType, targetCraft.LabCraft));
                }
                return;
            }

            if (toLabContainer.GetSubstancesCount() == 0)
            {
                if (CraftTools.TryAdd(fromLabContainer, toLabContainer, out LabSubstance transferSubstance))
                {
                    _gameManager.Game.CompleteTask(new AddSubstanceLabActivity(
                        fromLabContainer.ContainerType,
                        toLabContainer.ContainerType,
                        transferSubstance.SubstanceProperty));   
                }
                return;
            }
        }

        private IEnumerator StartDelayBetweenActivated()
        {
            _isAlreadyTriggered = true;
            yield return new WaitForSeconds(DELAY_TRIGGERED);
            _isAlreadyTriggered = false;
        }
    }
}