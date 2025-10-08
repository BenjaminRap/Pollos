using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class RotationManager : MonoBehaviourSingleton<RotationManager>
{
	[SerializeField] private Face	_currentFace;
	[SerializeField] private float	_rotationDuration = 0.2f;
	[SerializeField] private Transform	_rotatableChild;

	private Coroutine	_rotateCoroutine;
	private Quaternion	_globalRotation;
	private Quaternion	_localRotation;
	private Rotatable[]	_rotatablesObjets;
	private Cube	_cube;
	private Vector3Int	_rotationAxis;
	public Transform	RotatableChild { get => _rotatableChild; }
	public Quaternion	LocalRotation { get => _localRotation; }

    protected override void Awake()
    {
		base.Awake();
		_rotationAxis = Vector3Int.zero;
		_cube = GetComponent<Cube>();
        _globalRotation = Quaternion.identity;
		_localRotation = Quaternion.identity;
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
    }

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
		_rotationAxis = Vector3Int.zero;
	}

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
		if (_rotationAxis.x != 0 || _rotationAxis.y != 0)
			return ;
		_rotationAxis = Vector3Int.forward;
		Quaternion	rotation = Quaternion.AngleAxis(-axisValue * 90, Vector3.forward);
		_globalRotation = rotation * _globalRotation;
		_localRotation *= Quaternion.Inverse(rotation);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(_currentFace));
	}
	
	public void	RotateCube(Vector2Int value)
	{
		if (_rotationAxis != Vector3Int.zero)
			return ;
		Quaternion	rotation;
		if (value.y != 0)
			rotation = Quaternion.AngleAxis(-value.y * 90, Vector3.right);
		else
			rotation = Quaternion.AngleAxis(value.x * 90, Vector3.up);
		Quaternion	newLocalRotation = _localRotation * Quaternion.Inverse(rotation);
		Face		newFace = _cube.GetFace(newLocalRotation);
		if (newFace == null)
			return ;
		_rotationAxis = (value.y != 0) ? Vector3Int.right : Vector3Int.up;
		_localRotation = newLocalRotation;
		_globalRotation = rotation * _globalRotation;
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(newFace));
	}
}
