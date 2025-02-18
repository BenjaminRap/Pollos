using System.Collections;
using UnityEngine;

/// <summary>This class allows a GameObject with a rigidbody to freeze and unfreeze, not
/// losing the velocity. The velocity is reduced by a constant factor</summary>
[RequireComponent(typeof(Rigidbody))]
public class Rotatable : MonoBehaviour
{
	/// <summary>When rotating the level, the rotables position will be set to
	/// the nearest grid. The greater this variable, the greater the chance
	/// that this gameObject moves to the grid cell in the direction of the
	/// velocity.</summary>
	[SerializeField]
	[Range(0.0f, 0.25f)]
	private float			_adjustmentLength;

	/// <summary>When rotating, the velocity of the gameObject is multiplied
	/// by this value.</summary>
	private const float		_velocityMultiplicatorAtRotation = 0.4f;
	private Rigidbody		_rigidbody;
	private bool			_isFroze;
	private Vector3			_velocityAtFreeze;
	private Coroutine		_placeInGridCoroutine;
	
	public bool				IsFroze { get => _isFroze; }

	private void	Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_isFroze = false;
		_velocityAtFreeze = Vector3.zero;
	}

	/// <summary>Returns the middle of the nearest case. The result of this
	/// function is influenced by the _adjustmentLength.</summary>
	public Vector3		GetNearestGridCell(Vector3 adjustment)
	{
		Vector3	newPosition = transform.localPosition + adjustment.normalized * _adjustmentLength;

		newPosition.x = Mathf.Round(newPosition.x + 0.5f) - 0.5f;
		newPosition.y = Mathf.Round(newPosition.y + 0.5f) - 0.5f;
		return (newPosition);
	}

	/// <summary>Moves this rigidbody to the nearest grid cell, in maximum
	/// rotationDuration seconds.</summary>
	private IEnumerator	MoveToNearestGridCell(float rotationDuration)
	{
		Vector3	newPosition = GetNearestGridCell(_velocityAtFreeze);
		float	distance = Vector3.Distance(newPosition, transform.localPosition);
		float	speed = _velocityAtFreeze.magnitude + 0.01f;
		float	movementDuration = Mathf.Min(rotationDuration, distance / speed);
		
		yield return TransformUtils.LocalMoveInTime(transform, newPosition, movementDuration);
		_placeInGridCoroutine = null;
	}
	
	/// <summary>Freezes the rigidbody and move this object to the nearest grid
	/// in maximum rotationDuration seconds</summary>
	public void	Freeze(float rotationDuration)
	{
		if (_isFroze)
			return ;
		_isFroze = true;
		_velocityAtFreeze = _rigidbody.linearVelocity;
		_rigidbody.linearVelocity = Vector3.zero;
		_rigidbody.useGravity = false;
		_placeInGridCoroutine ??= StartCoroutine(MoveToNearestGridCell(rotationDuration));
	}

	/// <summary>Unfreeze the rigidbody</summary>
	public void	Unfreeze()
	{
		if (!_isFroze)
			return ;
		UpdateGravityUse();
		_isFroze = false;
		_rigidbody.linearVelocity = _velocityAtFreeze * _velocityMultiplicatorAtRotation;
	}
	
	public void	UpdateGravityUse()
	{
		bool	useGravity = (transform.parent.forward != Vector3.down && transform.parent.forward != Vector3.up);
		if (useGravity == false)
		{
			_rigidbody.linearVelocity = Vector3.zero;
			_velocityAtFreeze = Vector3.zero;
		}
		if (_rigidbody.useGravity != useGravity)
			_rigidbody.useGravity = useGravity;	
	}
}
