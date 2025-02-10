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
	
	public static bool	TryAndGetInstance(out InputManager inputManager)
	{
		inputManager = _instance;
		if (_instance == null)
		{
			Debug.LogError("The InputManager class has no instance !");
			return (false);
		}
		return (true);
	}
	
	public void	SetWorldActionsState(bool enabled)
	{
		if (!enabled)
			_worldActions.Disable();
		else
			_worldActions.Enable();
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
		if (!GameManager.TryAndGetInstance(out GameManager gameManager)
			|| !CharacterControler.TryAndGetInstance(out CharacterControler characterControler))
		{
			return ;
		}
		AnimatorClipInfo[] clipInfos = characterControler.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
		if (clipInfos.Length != 0 && clipInfos[0].clip.name != "SpawnAnim")
			gameManager.Defeat();
	}

	private void    Rotate(InputAction.CallbackContext context)
	{
		if (!Level.TryAndGetInstance(out Level currentLevel))
			return ;
		float value = Mathf.Round(context.ReadValue<float>());
		currentLevel.Rotate(value);
	}
}
