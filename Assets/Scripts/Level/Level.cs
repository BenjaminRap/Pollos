using UnityEngine;

[RequireComponent(typeof(StormManager))]
[RequireComponent(typeof(RotationManager))]
/// <summary>This class manages a level : the storms and the rotation of the level.</summary>
public class Level : MonoBehaviour
{
	private static Level	_instance;
	
	private StormManager	_stormManager;
	private RotationManager	_rotationManager;

	private void		Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instance of the level class !");
			Destroy(this);
			return ;
		}
		_instance = this;
		_stormManager = GetComponent<StormManager>();
		_rotationManager = GetComponent<RotationManager>();
	}
	
	/// <summary>Get the instance of this singleton if there is one.</summary>
	/// <param name="level">This variable will be set to the instance value.</param>
	/// <returns>True if there is an instance, false otherwise.</returns>
	public static bool	TryGetInstance(out Level level)
	{
		level = _instance;
		if (_instance == null)
		{
			Debug.LogError("The class Level has no instance !");
			return (false);
		}
		return (true);
	}
	
	/// <summary>Rotate the level.</summary>
	/// <param name="axisValue">The axis of the input : 1 for right and -1 for left.</param>
	public void			Rotate(int axisValue)
	{
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return ;
		characterControler.RotateCharacter();
		_stormManager.ComeCloser();
		_rotationManager.Rotate(axisValue);
	}
	
	public Transform	GetRotatableChild()
	{
		return (_rotationManager.RotatableChild);
	}
}
