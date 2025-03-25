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
        private bool _isPressed = false;
        private UserController _userController;
        private void Awake()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            _player = gameManager.PlayerSpawner.Player;
            _userController = _player.transform.GetComponent<UserController>();
        }
        
        private void OnEnable()
        {
            _grabInteractable.GrabbedEvent += OnGrab;
            _grabInteractable.UngrabbedEvent += OnUngrab;
            _userController.ButtonClicked += OnButtonClick;
        }

        private void OnDisable()
        {
            _grabInteractable.GrabbedEvent -= OnGrab;
            _grabInteractable.UngrabbedEvent -= OnUngrab;
            _userController.ButtonClicked -= OnButtonClick;
        }

        private void Update()
        {
            if (_isActive)
            {
                _panel.rotation = Quaternion.LookRotation(_panel.position - _player.transform.position, new Vector3(0, 1, 0));
                _panel.rotation = Quaternion.Euler(0, _panel.rotation.eulerAngles.y, 0);
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
            _isPressed = false;
        }

        private void OnButtonClick()
        {
            if (!_isActive)
            {
                return;
            }
            if (_labContainer.GetSubstancesCount() == 0)
            {
                _panel.gameObject.SetActive(false);
                return;
            }
            _isPressed = !_isPressed;
            _panel.gameObject.SetActive(_isPressed);
            _text.text = _labContainer.GetTopSubstance().SubstanceProperty.HintName;
        }
    } 
}
