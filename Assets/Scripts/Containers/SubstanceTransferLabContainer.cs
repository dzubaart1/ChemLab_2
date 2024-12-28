using System.Collections;
using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using Core;
using Core.Services;
using Crafting;
using LocalManagers;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Containers
{
    public class SubstanceTransferLabContainer : MonoBehaviour
    {
        private const float DELAY_TRIGGERED = 0.5f;
        
        [SerializeField] private LabContainer _labContainer;
        
        private bool _isAlreadyTriggered;
        
        private void OnTriggerStay(Collider other)
        {
            VRGrabInteractable vrGrabInteractable = _labContainer.GetComponentInChildren<VRGrabInteractable>();
            if (vrGrabInteractable == null)
            {
                return;
            }
            
            if (vrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }
            
            IXRSelectInteractor interactor = vrGrabInteractable.interactorsSelecting[0];
            if(interactor is null)
            {
                return;
            }
            
            ActionBasedController controller = interactor.transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                return;
            }

            CupSocketLabContainer cup = _labContainer.GetComponent<CupSocketLabContainer>();
            if (cup != null && cup.IsClosed())
            {
                return;
            }
            
            SubstanceTransferLabContainer targetSubstanceTransferLabContainer = other.GetComponent<SubstanceTransferLabContainer>();
            if(targetSubstanceTransferLabContainer is null)
            {
                return;
            }

            CupSocketLabContainer targetCupSocketLabContainerCup = other.GetComponent<CupSocketLabContainer>();
            if(targetCupSocketLabContainerCup != null && targetCupSocketLabContainerCup.IsClosed())
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
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            BaseLocalManager localManager = gameManager.CurrentBaseLocalManager;
            if (localManager == null)
            {
                return;
            }
            
            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() == 0 &
                toLabContainer.GetSubstancesCount() != 0)
            {
                if (CraftTools.TryAdd(toLabContainer, fromLabContainer, out LabSubstance transferSubstance))
                {
                    localManager.OnActivityComplete(new AddSubstanceLabActivity(
                        toLabContainer.ContainerType,
                        fromLabContainer.ContainerType,
                        transferSubstance.SubstanceProperty));   
                }
                return;
            }

            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() != 0 &
                toLabContainer.GetSubstancesCount() != 0)
            {
                if (CraftTools.TryFindCraft(localManager.GetSOCrafts(), fromLabContainer.GetSubstanceProperties().Concat(toLabContainer.GetSubstanceProperties()).ToList(),ECraft.Mix ,out SOLabCraft targetCraft))
                {
                    CraftTools.Mix(targetCraft.LabCraft, fromLabContainer, toLabContainer);
                    localManager.OnActivityComplete(new CraftSubstanceLabActivity(toLabContainer.ContainerType, targetCraft.LabCraft));
                }
                return;
            }

            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() != 0 &
                toLabContainer.GetSubstancesCount() == 0)
            {
                if (CraftTools.TryAdd(fromLabContainer, toLabContainer, out LabSubstance transferSubstance))
                {
                    localManager.OnActivityComplete(new AddSubstanceLabActivity(
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
                if (CraftTools.TryFindCraft(localManager.GetSOCrafts(), fromLabContainer.GetSubstanceProperties().Concat(toLabContainer.GetSubstanceProperties()).ToList(),ECraft.Mix ,out SOLabCraft targetCraft))
                {
                    CraftTools.Mix(targetCraft.LabCraft, fromLabContainer, toLabContainer);
                    localManager.OnActivityComplete(new CraftSubstanceLabActivity(toLabContainer.ContainerType, targetCraft.LabCraft));
                }
                return;
            }

            if (toLabContainer.GetSubstancesCount() == 0)
            {
                if (CraftTools.TryAdd(fromLabContainer, toLabContainer, out LabSubstance transferSubstance))
                {
                    localManager.OnActivityComplete(new AddSubstanceLabActivity(
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