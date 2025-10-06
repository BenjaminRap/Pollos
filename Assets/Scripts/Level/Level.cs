using UnityEngine;

[RequireComponent(typeof(StormManager))]
[RequireComponent(typeof(RotationManager))]
public class Level : MonoBehaviour
{
	private static Level	_instance;
	
	private StormManager	_stormManager;
	private RotationManager	_rotationManager;

	private void	Awake()
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
	
	public void	RotateFace(int axisValue)
	{
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return ;
		characterControler.RotateCharacter();
		_stormManager.ComeCloser();
		_rotationManager.RotateFace(axisValue);
	}
	
	public void	RotateCube(Vector2Int axisValue)
	{
		if (axisValue.x != 0 && axisValue.y != 0)
			return ;
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return ;
		characterControler.RotateCharacter();
		_rotationManager.RotateCube(axisValue);
	}
	
	public Transform	GetRotatableChild()
	{
		return (_rotationManager.RotatableChild);
	}
}
