using System.Collections;
using UnityEngine;

public class RotablesManager : MonoBehaviour
{
	/// <summary>The angle rotated each input.</summary>
	private const float		_rotationAngle = 90.0f;
	
	/// <summary>The time it takes for the level to rotate, it shouldn't be greater 
	/// than the rotate animation.</summary>
	[SerializeField]
	private float			_rotationDuration = 0.2f;
	/// <summary>The GameOBject that will be rotated.</summary>
	[SerializeField]
	private Transform		_rotatableChild;

	private Coroutine		_rotateCoroutine;
	private Quaternion		_rotationGoal;
	private Rotatable[]		_rotatablesObjets;
	
	public Transform		RotatableChild { get => _rotatableChild; }

    private void Start()
    {
        _rotationGoal = _rotatableChild.rotation;
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
    }

	/// <summary>A corotutine that rotates the level and restart the rigidbody's
	/// simulation after that</summary>
	private IEnumerator	RotateLevelToRotationGoal()
	{
		
		yield return TransformUtils.RotateInTime(_rotatableChild, _rotationGoal, _rotationDuration);
		UnfreezeRotatables();
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
	
	public void	Rotate(int axisValue)
	{
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		FreezeRotatables();
		_rotationGoal *= Quaternion.Euler(-axisValue * _rotationAngle * Vector3.forward);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal());
	}
}
