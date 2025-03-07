using Containers;
using UnityEngine;
using Core;
using TMPro;
using UI.Components;
using Mechanics;

namespace UI
{
    public class SubstancePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private LabContainer _labContainer;
        [SerializeField] private Transform _panel;
        [SerializeField] private VRGrabInteractable _grabInteractable;

        private Player _player;
        private bool _isActive = false;
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            _player = gameManager.PlayerSpawner.Player;
            _player.LeftHandGrabbedEvent += OnGrab;
            _player.RightHandGrabbedEvent += OnGrab;
        }
        
        private void OnEnable()
        {
            _grabInteractable.UngrabbedEvent += OnUngrab;
        }

        private void OnDisable()
        {
            _player.LeftHandGrabbedEvent -= OnGrab;
            _player.RightHandGrabbedEvent -= OnGrab;
            _grabInteractable.UngrabbedEvent -= OnUngrab;
        }

        private void Update()
        {
            if (_isActive)
            {
                if (_labContainer.GetSubstancesCount() == 0)
                {
                    _panel.gameObject.SetActive(false);
                    return;
                }
                
                _panel.gameObject.SetActive(true);
                
                _panel.rotation = Quaternion.LookRotation(_panel.position - _player.transform.position, new Vector3(0, 1, 0));
                _panel.rotation = Quaternion.Euler(0, _panel.rotation.eulerAngles.y, 0);

                _text.text = _labContainer.GetTopSubstance().SubstanceProperty.NameForPanel;
            }
        }

        private void OnGrab()
        {
            _isActive = true;
        }
        
        private void OnUngrab()
        {
            _panel.gameObject.SetActive(false);
            _isActive = false;
        }
    } 
}
