using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private static InputManager _instance;
	
	[SerializeField]
	private GameObject			_ui;

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
		_playerController.World.resetLevel.performed += ResetLevel;
		_playerController.World.menu.performed += OpenMenu;
		_instance = this;
	}

	private void    OnDestroy()
	{
		_playerController.World.Disable();
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
