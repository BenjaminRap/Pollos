using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class RotationManager : MonoBehaviourSingleton<RotationManager>
{
	[SerializeField] private float	_rotationDuration = 0.2f;

	private Face	_currentFace;
	private Coroutine	_rotateCoroutine;
	private Quaternion	_rotation = Quaternion.identity;
	private Rotatable[]	_rotatablesObjets;
	private Cube	_cube;
	private Vector3Int	_rotationAxis = Vector3Int.zero;

    protected override void Awake()
    {
		base.Awake();
		_cube = GetComponent<Cube>();
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
    }

	private void	Start()
	{
		_cube.TryGetFace(Quaternion.identity, out _currentFace);
	}

	private IEnumerator	RotateLevelToRotationGoal(Face newFace)
	{
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		FreezeRotatables();
		if (newFace != _currentFace)
			newFace.SetRendered(true);
		yield return TransformUtils.RotateInTime(transform, _rotation, _rotationDuration);
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
		_rotation = rotation * _rotation;
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(_currentFace));
	}
	
	public void	RotateCube(Vector2Int value)
	{
		if (_rotationAxis != Vector3Int.zero)
			return ;
		Quaternion	rotation = (value.y != 0)
			? Quaternion.AngleAxis(value.y * 90f, Vector3.left)
			: Quaternion.AngleAxis(value.x * 90f, Vector3.up);
		Quaternion	newGlobalRotation = rotation * _rotation;

		if (!_cube.TryGetFace(Quaternion.Inverse(newGlobalRotation), out Face newFace))
			return ;
		_rotationAxis = (value.y != 0) ? Vector3Int.right : Vector3Int.up;
		_rotation = newGlobalRotation;
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(newFace));
	}
}
