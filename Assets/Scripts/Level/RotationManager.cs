using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

	/// <summary>Rotate the level around the z axis, that means that we can
	/// see the same face.</summary>
	/// <param name="axisValue"> This is the value of the input of the player</param>
	public void	RotateFace(int axisValue)
	{
		if (_rotationAxis.x != 0 || _rotationAxis.y != 0)
			return ;
		_rotationAxis = Vector3Int.forward;
		Quaternion	rotation = Quaternion.AngleAxis(-axisValue * 90, Vector3.forward);
		_globalRotation = rotation * _globalRotation;
		_localRotation *= Quaternion.Inverse(rotation);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(_faceRotationDuration));
		_onRotateEvents.Invoke();
	}
	
	/// <summary>Rotate the level around the x or y axis, that means that the current
	/// face will change.</summary>
	/// <param name="value"> This is the value of the input of the player</param>
	public Face	RotateCube(Vector2Int value)
	{
		if (_rotationAxis != Vector3Int.zero)
			return (null);
		Quaternion	rotation;
		if (value.y != 0)
			rotation = Quaternion.AngleAxis(-value.y * 90, Vector3.right);
		else
			rotation = Quaternion.AngleAxis(value.x * 90, Vector3.up);
		Quaternion	newLocalRotation = _localRotation * Quaternion.Inverse(rotation);
		Face		newFace = _cube.GetFace(newLocalRotation);
		if (newFace == null)
			return (null);
		_rotationAxis = (value.y != 0) ? Vector3Int.right : Vector3Int.up;
		_localRotation = newLocalRotation;
		_globalRotation = rotation * _globalRotation;
		_rotateCoroutine = StartCoroutine(RotateCubeToRotationGoal(newFace, _cubeRotationDuration));
		_onRotateEvents.Invoke();
		return (newFace);
	}
}
