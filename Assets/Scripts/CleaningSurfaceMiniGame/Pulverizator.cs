using System;
using BioEngineerLab.Activities;
using Core;
using Mechanics;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Database;

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
            public String TargetName;
        }

        [Header("Refs")]
        [SerializeField] private VRGrabInteractable _vrGrabInteractable;
        [SerializeField] private Transform _rayOrigin;
        [SerializeField] private XRInteractorLineVisual _lineVisual;
        
        [Space]
        [Header("UI Elements")] 
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _panel;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private Gradient _handsGradient;
        [SerializeField] private Gradient _surfaceGradient;
        [SerializeField] private Gradient _otherGradient;
        
        [Space]
        [SerializeField] private TagConfig[] _tagConfigs;
        
        private float _timerDelay = 1f;
        private bool _isTimerActive = false;
        private float _timer = 0f;
        
        private bool _isAlreadyTriggered = false;
        private Player _player;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            _player = gameManager.PlayerSpawner.Player;
        }
        private void Update()
        {
            if (_isTimerActive)
            {
                _timer += Time.deltaTime;

                if (_timer > _timerDelay)
                {
                    _isTimerActive = false;
                }
            }
            
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
            
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            if (gameManager.IsSoundsOn)
            {
                AudioClip a = ResourcesDatabase.ReadSound("Pulverizator");
                AudioSource.PlayClipAtPoint(a, transform.position, 0.6f);
            }
            
            Ray ray = new Ray(_rayOrigin.transform.position, _rayOrigin.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CheckRaycastHit(hit);
            }
            else
            {
                _panel.gameObject.SetActive(false);
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

            if (_isTimerActive)
            {
                return;
            }

            if (tagConfig.TargetType == EPulverizatorTarget.RightHandHit ||
                tagConfig.TargetType == EPulverizatorTarget.LeftHandHit)
            {
                Ray ray = new Ray(hit.point + _rayOrigin.transform.forward.Multiply(new Vector3(0.01f, 0.01f, 0.01f)), _rayOrigin.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit2))
                {
                    if (TryGetTagConfig(hit2.collider.gameObject.tag, out TagConfig tagConfig2))
                    {
                        gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig2.TargetType));
                        RestartTimer();
                        return;
                    }
                    else
                    {
                        gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig.TargetType));
                        RestartTimer();
                        return;
                    }
                }
            }
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new PulverizatorLabActivity(tagConfig.TargetType));
            RestartTimer();
        }
        
        private void ChangeColor(RaycastHit hit)
        {
            if (TryGetTagConfig(hit.collider.gameObject.tag, out TagConfig tagConfig))
            {
                _panel.gameObject.SetActive(true);
                _panel.rotation = Quaternion.LookRotation(_panel.position - _player.transform.position, new Vector3(0, 1, 0));
                _panel.rotation = Quaternion.Euler(0, _panel.rotation.eulerAngles.y, 0);
                if (tagConfig.TargetType == EPulverizatorTarget.RightHandHit ||
                    tagConfig.TargetType == EPulverizatorTarget.LeftHandHit)
                {
                    Ray ray = new Ray(hit.point + _rayOrigin.transform.forward.Multiply(new Vector3(0.01f, 0.01f, 0.01f)), _rayOrigin.transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit2))
                    {
                        if (TryGetTagConfig(hit2.collider.gameObject.tag, out TagConfig tagConfig2) &&
                            tagConfig2.TargetType != EPulverizatorTarget.LeftHandHit &&
                            tagConfig2.TargetType != EPulverizatorTarget.RightHandHit)
                        {
                            _lineVisual.validColorGradient = _otherGradient;
                            _text.text = tagConfig2.TargetName;
                        }
                        else
                        {
                            _lineVisual.validColorGradient = _handsGradient;
                            _text.text = tagConfig.TargetName;
                        }
                    }
                }
                else if (tagConfig.TargetType == EPulverizatorTarget.CleaningSurface)
                {
                    _lineVisual.validColorGradient = _surfaceGradient;
                    _text.text = tagConfig.TargetName;
                }
                else
                {
                    _lineVisual.validColorGradient = _otherGradient;
                    _text.text = tagConfig.TargetName;
                }
            }

            else {
                _panel.gameObject.SetActive(false);
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
                    return true;
                }
            }

            return false;
        }
        
        private void RestartTimer()
        {
            _isTimerActive = true;
            _timer = 0f;
        }
    }
}