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
	[SerializeField]
	private float					_cubeRotationDuration = 0.4f;
	[SerializeField]
	private Transform				_rotatableChild;
	[SerializeField]
	private UnityEvent				_onRotateEvents;

	private Coroutine				_rotateCoroutine;
	private Quaternion				_globalRotation;

	private Rotatable[]				_rotatablesObjets;
	private Cube					_cube;
	
	public Transform				RotatableChild { get => _rotatableChild; }
	public Cube						Cube { get => _cube; }

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
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
    }

	/// <summary>A corotutine that rotates the level to the _rotationGoal rotation
	/// And freeze and unfreeze the rigidbody s inside the Level.</summary>
	private IEnumerator	RotateLevelToRotationGoal(float rotationDuration)
	{
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		foreach (Rotatable rotatable in _rotatablesObjets)
			rotatable.Freeze();
		yield return (new WaitForFixedUpdate());
		yield return (new WaitForFixedUpdate());
		yield return (TransformUtils.RotateInTime(_rotatableChild, _globalRotation, rotationDuration));
		foreach (Rotatable rotatable in _rotatablesObjets)
			rotatable.Unfreeze();
		_rotateCoroutine = null;
	}
	
	/// <summary>A coroutine that rotates the level to the rotation goal but also
	/// hides the faces that aren't facing the camera.</summary>
	private IEnumerator	RotateCubeToRotationGoal(Face newFace, float rotationDuration)
	{
		Face	previousFace = _currentFace;
		newFace.SetRendered(true);
		_currentFace = newFace;
		yield return RotateLevelToRotationGoal(rotationDuration);
		previousFace.SetRendered(false);
	}

	public Quaternion	GetRotationFromInput(Vector3Int input)
	{
		if (!VectorUtils.IsAxis(input))
			return (Quaternion.identity);
		Vector3Int	rotationAxis;
		if (input.z != 0)
			rotationAxis = Vector3Int.back * input.z;
		else if (input.y != 0)
			rotationAxis = Vector3Int.left * input.y;
		else
			rotationAxis = Vector3Int.up * input.x;
		return (Quaternion.AngleAxis(90, rotationAxis));
	}
	
	public Face	Rotate(Vector3Int input)
	{
		Quaternion	rotation = GetRotationFromInput(input);
		if (rotation == Quaternion.identity)
			return (null);
		Face		newFace = _cube.GetFace(Quaternion.Inverse(rotation) * _currentFace.transform.forward);
		if (newFace == null)
			return (null);
		_globalRotation = rotation * _globalRotation;
		if (newFace == _currentFace)
			_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(_faceRotationDuration));
		else
			_rotateCoroutine = StartCoroutine(RotateCubeToRotationGoal(newFace, _cubeRotationDuration));
		_onRotateEvents.Invoke();
		return newFace;
	}
}
