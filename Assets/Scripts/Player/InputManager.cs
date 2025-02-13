using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>This class is a singleton that manages the inputs, 
/// they all come through this class first, and then this class calls the
/// different functions.</summary>
public class InputManager : MonoBehaviour
{
	private static InputManager 			_instance;
	
	[SerializeField]
	private GameObject						_ui;

	/// <summary>The default acton map.</summary>
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
	
	/// <summary>Get the instance of this singleton if there is one.</summary>
	/// <param name="inputManager">This variable will be set to the instance value.</param>
	/// <returns>True if there is an instance, false otherwise.</returns>
	public static bool	TryGetInstance(out InputManager inputManager)
	{
		inputManager = _instance;
		if (_instance == null)
		{
			Debug.LogError("The InputManager class has no instance !");
			return (false);
		}
		return (true);
	}
	
	/// <summary>Change the activate state of the default action map.</summary>
	/// <param name="enabled">True if the function should activate the action
	/// map, false if it should deactivate it.</param>
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
		if (!GameManager.TryGetInstance(out GameManager gameManager)
			|| !PollosController.TryGetInstance(out PollosController characterControler))
		{
			return ;
		}
		AnimatorClipInfo[] clipInfos = characterControler.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
		if (clipInfos.Length != 0 && clipInfos[0].clip.name != "SpawnAnim")
			gameManager.Defeat();
	}

	private void    Rotate(InputAction.CallbackContext context)
	{
		if (!Level.TryGetInstance(out Level currentLevel))
			return ;
		int value = Mathf.RoundToInt(context.ReadValue<float>());
		currentLevel.Rotate(value);
	}
}
