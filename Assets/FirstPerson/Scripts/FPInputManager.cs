using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;


public class FPInputManager : MonoBehaviour, PlayerInputs.IFirstPersonPlayerActions
{
    [SerializeField] private FPPlayer _player;

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnMoveForward(InputAction.CallbackContext context)
    {
        Debug.Log("hi");
        if (context.started || context.performed)
        {
            _player.IsMovingForward = true;
        }
        else if (context.canceled)
        {
            _player.IsMovingForward = false;
        }
    }
}
