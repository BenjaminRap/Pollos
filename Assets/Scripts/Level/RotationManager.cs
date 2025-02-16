using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(Cube))]
/// <summary>This class is a singleton than manages the rotation of the level.
/// It rotate the parent rotable and freeze the rotables in it.</summary>
public class RotationManager : MonoBehaviour
{
	private static RotationManager	_instance;

	/// <summary>The angle rotated each input.</summary>
	private const float				_rotationAngle = 90.0f;
	
	[SerializeField]
	private Face					_currentFace;
	
	/// <summary>The time it takes for the level to rotate, it shouldn't be greater 
	/// than the rotate animation.</summary>
	[SerializeField]
	private float					_rotationDuration = 0.2f;
	/// <summary>The GameOBject that will be rotated.</summary>
	[SerializeField]
	private Transform				_rotatableChild;

	private Coroutine				_rotateCoroutine;
	private Quaternion				_globalRotation;
	private Quaternion				_localRotation;

	private Rotatable[]				_rotatablesObjets;
	private Cube					_cube;
	
	public Transform				RotatableChild { get => _rotatableChild; }

    private void Start()
    {
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the RotationManager class !");
			Destroy(gameObject);
			return ;
		}
		_instance = this;

		_cube = GetComponent<Cube>();
        _globalRotation = Quaternion.identity;
		_localRotation = Quaternion.identity;
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
    }

	/// <summary>A corotutine that rotates the level and restart the rigidbody's
	/// simulation after that</summary>
	private IEnumerator	RotateLevelToRotationGoal(Face newFace)
	{
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		FreezeRotatables();
		if (newFace != _currentFace)
			newFace.SetRendered(true);
		yield return TransformUtils.RotateInTime(_rotatableChild, _globalRotation, _rotationDuration);
		UnfreezeRotatables();
		if (newFace != _currentFace)
		{
			_currentFace.SetRendered(false);
			_currentFace = newFace;
		}
		_rotateCoroutine = null;
	}

	/// <summary>Toggle the freezing state of the rotatables objects, freeze
	/// means they won't move with forces.</summary>
	private void	FreezeRotatables()
	{
		foreach (Rotatable rotatable in _rotatablesObjets)
		{
			rotatable.Freeze(_rotationDuration);
		}
	}

	private void	UnfreezeRotatables()
	{
		foreach (Rotatable rotatable in _rotatablesObjets)
		{
			rotatable.Unfreeze();
		}
	}
	
	public void	RotateFace(int axisValue)
	{
		Quaternion	rotation = Quaternion.AngleAxis(-axisValue * _rotationAngle, Vector3.forward);
		_globalRotation = rotation * _globalRotation;
		_localRotation *= Quaternion.Inverse(rotation);
		_cube.ShowPossibleRotations(_localRotation);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(_currentFace));
	}
	
	public void	RotateCube(Vector2Int value)
	{
		Quaternion rotation;
		if (value.y != 0)
			rotation = Quaternion.AngleAxis(-value.y * _rotationAngle, Vector3.right);
		else
			rotation = Quaternion.AngleAxis(value.x * _rotationAngle, Vector3.up);
		Face	newFace = _cube.GetFace(_localRotation, Quaternion.Inverse(rotation));
		if (newFace == null)
			return ;
		_localRotation *= Quaternion.Inverse(rotation);
		_cube.ShowPossibleRotations(_localRotation);
		_globalRotation = rotation * _globalRotation;
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(newFace));
	}
}
