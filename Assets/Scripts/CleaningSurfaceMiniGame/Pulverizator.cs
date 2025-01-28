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
        [SerializeField] private XRInteractorLineVisual _lineVisual;
        
        [Header("Configs")]
        [SerializeField] private Gradient _handsGradient;
        [SerializeField] private Gradient _surfaceGradient;
        [SerializeField] private Gradient _otherGradient;
        
        [Space]
        [SerializeField] private TagConfig[] _tagConfigs;
        
        private bool _isAlreadyTriggered = false;
        
        private void Update()
        {
            Ray colorRay = new Ray(_rayOrigin.transform.position, _rayOrigin.transform.forward);
            if (Physics.Raycast(colorRay, out RaycastHit colorHit))
            {
                ChangeColor(colorHit);
            }
            
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
            
            Ray ray = new Ray(_rayOrigin.transform.position, _rayOrigin.transform.forward);
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

            if (tagConfig.TargetType == EPulverizatorTarget.RightHandHit ||
                tagConfig.TargetType == EPulverizatorTarget.LeftHandHit)
            {
                Ray ray = new Ray(hit.point, _rayOrigin.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit2))
                {
                    if (TryGetTagConfig(hit2.collider.gameObject.tag, out TagConfig tagConfig2))
                    {
                        gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig2.TargetType));
                        return;
                    }
                    else
                    {
                        gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig.TargetType));
                        return;
                    }
                }
            }
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig.TargetType));
        }
        
        private void ChangeColor(RaycastHit hit)
        {
            if (TryGetTagConfig(hit.collider.gameObject.tag, out TagConfig tagConfig))
            {
                if (tagConfig.TargetType == EPulverizatorTarget.RightHandHit ||
                    tagConfig.TargetType == EPulverizatorTarget.LeftHandHit)
                {
                    Ray ray = new Ray(hit.point, _rayOrigin.transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit2))
                    {
                        if (TryGetTagConfig(hit2.collider.gameObject.tag, out TagConfig tagConfig2) &&
                            tagConfig2.TargetType != EPulverizatorTarget.LeftHandHit &&
                            tagConfig2.TargetType != EPulverizatorTarget.RightHandHit)
                        {
                            _lineVisual.validColorGradient = _otherGradient;
                        }
                        else
                        {
                            _lineVisual.validColorGradient = _handsGradient;
                        }
                    }
                }
                else if (tagConfig.TargetType == EPulverizatorTarget.CleaningSurface)
                {
                    _lineVisual.validColorGradient = _surfaceGradient;
                }
                else
                {
                    _lineVisual.validColorGradient = _otherGradient;
                }
            }
        }

        private bool TryGetTagConfig(string tag, out TagConfig targetTagConfig)
        {
            targetTagConfig = null;
            
            foreach (var tagConfig in _tagConfigs)
            {
                if (tagConfig.Tag == tag)
                {
                    targetTagConfig = tagConfig;
                    Debug.Log(targetTagConfig.Tag);
                    return true;
                }
            }

            return false;
        }
    }
}