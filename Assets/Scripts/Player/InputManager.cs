using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private static InputManager _instance;

	private PlayerController    _playerController;

	private void    Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the Inputmanager class !");
			Destroy(this);
			return ;
		}
		_playerController = new PlayerController();

		_playerController.World.Enable();
		_playerController.World.rotateWorld.performed += Rotate;
		_instance = this;
	}

	private void    OnDestroy()
	{
		_playerController.World.Disable();
	}

	private void    Rotate(InputAction.CallbackContext context)
	{
		Level	currentLevel = Level.GetInstance();
		
		if (currentLevel == null)
		{
			Debug.LogError("The class Level has no instance but the input Rotate has been performed");
			return ;
		}
		currentLevel.Rotate(context.ReadValue<float>());
	}
}
