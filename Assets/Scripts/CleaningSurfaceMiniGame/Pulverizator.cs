using System;
using BioEngineerLab.Activities;
using Core;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class Pulverizator: MonoBehaviour
    {
        [Serializable]
        private class TagConfig
        {
            public string Tag;
            public EPulverizatorTarget TargetType;
        }

        [Header("Refs")]
        [SerializeField] private VRGrabInteractable _vrGrabInteractable;
        [SerializeField] private Transform _rayOrigin;
        
        [Space]
        [SerializeField] private TagConfig[] _tagConfigs;
        
        private bool _isAlreadyTriggered = false;
        
        private void Update()
        {
            if (_vrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }

            ActionBasedController controller = _vrGrabInteractable.interactorsSelecting[0].transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                return;
            }

            if (!controller.activateAction.action.triggered)
            {
                return;
            }
            
            Ray ray = new Ray(_rayOrigin.transform.position, _rayOrigin.transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CheckRaycastHit(hit);
            }
        }

        private void CheckRaycastHit(RaycastHit hit)
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
            
            if (!TryGetTagConfig(hit.collider.gameObject.tag, out TagConfig tagConfig))
            {
                return;
            }

            if (tagConfig.TargetType == EPulverizatorTarget.CleaningSurface)
            {
                CleaningSurface cleaningSurface = hit.transform.GetComponent<CleaningSurface>();

                if (cleaningSurface == null)
                {
                    return;
                }
                
                cleaningSurface.OnPulverizatorHit(hit.point);
                return;
            }
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig.TargetType));
        }

        private bool TryGetTagConfig(string tag, out TagConfig targetTagConfig)
        {
            targetTagConfig = null;
            
            foreach (var tagConfig in _tagConfigs)
            {
                if (tagConfig.Tag == tag)
                {
                    targetTagConfig = tagConfig;
                    return true;
                }
            }

            return false;
        }
    }
}