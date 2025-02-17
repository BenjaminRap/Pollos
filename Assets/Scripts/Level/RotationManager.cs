using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelRotation
{
	private Quaternion	_localRotation;
	private Quaternion	_globalRotation;
	private	Vector3Int	_rotationAxis;
	private Face		_newFace;

	public LevelRotation(Quaternion localRotation, Quaternion globalRotation, Vector3Int rotationAxis, Face newFace)
	{
		_localRotation = localRotation;
		_globalRotation = globalRotation;
		_rotationAxis = rotationAxis;
		_newFace = newFace;
	}
	
	public Quaternion	LocalRotation { get => _localRotation; }
	public Quaternion	GlobalRotation { get => _globalRotation; }
	public Vector3Int	RotationAxis { get => _rotationAxis; }
	public Face			NewFace { get => _newFace; }
}

[RequireComponent(typeof(Cube))]
/// <summary>This class is a singleton than manages the rotation of the level.
/// It rotate the parent rotable and freeze the rotables in it.</summary>
public class RotationManager : MonoBehaviour
{
	private static RotationManager	_instance;

	[SerializeField]
	private Face					_currentFace;
	[SerializeField]
	private float					_faceRotationDuration = 0.2f;
	private float					_cubeRotationDuration = 0.4f;
	[SerializeField]
	private Transform				_rotatableChild;
	[SerializeField]
	private UnityEvent				_onRotateEvents;

	private Coroutine				_rotateCoroutine;
	private Quaternion				_globalRotation;
	private Quaternion				_localRotation;

	private Rotatable[]				_rotatablesObjets;
	private Cube					_cube;
	private Vector3Int				_rotationAxis;
	
	public Transform				RotatableChild { get => _rotatableChild; }
	public Quaternion				LocalRotation { get => _localRotation; }

    private void Start()
    {
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the RotationManager class !");
			Destroy(gameObject);
			return ;
		}
		_instance = this;

		_rotationAxis = Vector3Int.zero;
		_cube = GetComponent<Cube>();
        _globalRotation = Quaternion.identity;
		_localRotation = Quaternion.identity;
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
    }

	/// <summary>A corotutine that rotates the level to the _rotationGoal rotation
	/// And freeze and unfreeze the rigidbody s inside the Level.</summary>
	private IEnumerator	RotateLevelToRotationGoal(float rotationDuration)
	{
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		foreach (Rotatable rotatable in _rotatablesObjets)
			rotatable.Freeze(rotationDuration);
		yield return TransformUtils.RotateInTime(_rotatableChild, _globalRotation, rotationDuration);
		foreach (Rotatable rotatable in _rotatablesObjets)
			rotatable.Unfreeze();
		_rotateCoroutine = null;
		_rotationAxis = Vector3Int.zero;
	}
	
	/// <summary>A coroutine that rotates the level to the rotation goal but also
	/// hides the faces that aren't facing the camera.</summary>
	private IEnumerator	RotateCubeToRotationGoal(Face newFace, float rotationDuration)
	{
		newFace.SetRendered(true);
		yield return RotateLevelToRotationGoal(rotationDuration);
		_currentFace.SetRendered(false);
		_currentFace = newFace;
	}
	
	public LevelRotation	CanRotate(Vector3Int axis)
	{
		if (!VectorUtils.IsAxis(axis))
			return (null);
		Vector3Int	rotationAxis;
		if (axis.z != 0)
			rotationAxis = Vector3Int.back * axis.z;
		else if (axis.y != 0)
			rotationAxis = Vector3Int.left * axis.y;
		else
			rotationAxis = Vector3Int.up * axis.x;
		if (!(_rotationAxis == Vector3Int.zero
			|| _rotationAxis == rotationAxis
			|| _rotationAxis == -rotationAxis))
			return (null);
		Quaternion	rotation = Quaternion.AngleAxis(90, rotationAxis);
		Quaternion	localRotation = _localRotation * Quaternion.Inverse(rotation);
		Face		newFace = _cube.GetFace(localRotation);
		if (newFace == null)
			return (null);
		Quaternion	globalRotation = rotation * _globalRotation;
		return (new LevelRotation(localRotation, globalRotation, rotationAxis, newFace));
	}
	
	public Face	Rotate(LevelRotation levelRotation)
	{
		if (levelRotation == null)
			return (null);
		_rotationAxis = levelRotation.RotationAxis;
		_localRotation = levelRotation.LocalRotation;
		_globalRotation = levelRotation.GlobalRotation;
		if (levelRotation.NewFace == _currentFace)
			_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(_faceRotationDuration));
		else
			_rotateCoroutine = StartCoroutine(RotateCubeToRotationGoal(levelRotation.NewFace, _cubeRotationDuration));
		_onRotateEvents.Invoke();
		return (levelRotation.NewFace);
	}
}
