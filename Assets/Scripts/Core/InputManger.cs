using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputManger : MonoBehaviour, PlayerInputs.IPlayerActions
    {
        [SerializeField] private UserController _userController;

        [CanBeNull] private PlayerInputs _playerInputs;

        private void Start()
        {
            _playerInputs = new PlayerInputs();
            _playerInputs.Player.AddCallbacks(this);
            _playerInputs.Player.Enable();
        }

        public void OnLeftControllerPrimaryButtonClicked(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }
            
            _userController.ToggleTabletUI();
        }

        public void OnRightControllerPrimaryButtonClicked(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }
            
            _userController.ToggleSubstanceUI();
        }
    }
}