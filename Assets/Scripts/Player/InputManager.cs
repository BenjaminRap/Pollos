using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private static InputManager 			_instance;
	
	[SerializeField]
	private GameObject						_ui;

	private PlayerController.WorldActions    _worldActions;

	private void    Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the Inputmanager class !");
			Destroy(this);
			return ;
		}
		PlayerController playerController = new();
		_worldActions = playerController.World;

		_worldActions.Enable();
		_worldActions.rotateWorld.performed += Rotate;
		_worldActions.resetLevel.performed += ResetLevel;
		_worldActions.menu.performed += OpenMenu;
		_instance = this;
	}

	private void    OnDestroy()
	{
		_worldActions.Disable();
	}
	
	private void	OpenMenu(InputAction.CallbackContext context)
	{
		_ui.SetActive(!_ui.activeSelf);
	}
	
	private void	ResetLevel(InputAction.CallbackContext context)
	{
		GameManager	gameManager = GameManager.GetInstance();

		if (gameManager == null)
		{
			Debug.LogError("The GameManager class has no instance but the function resetLevel has been called !");
			return ;
		}
		gameManager.Defeat();
	}

	private void    Rotate(InputAction.CallbackContext context)
	{
		Level	currentLevel = Level.GetInstance();
		
		if (currentLevel == null)
		{
			Debug.LogError("The class Level has no instance but the input Rotate has been performed");
			return ;
		}
		float value = Mathf.Round(context.ReadValue<float>());
		currentLevel.Rotate(value);
	}
}
