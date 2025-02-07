using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerController    _playerController;
    private void    Awake()
    {
        _playerController = new PlayerController();

        _playerController.World.Enable();
        _playerController.World.rotateWorld.performed += Rotate;
    }
    
    private void    OnDestroy()
    {
        _playerController.World.Disable();
    }

    private void    Rotate(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());
    }
}
