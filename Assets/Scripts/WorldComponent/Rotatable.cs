using System.Collections;
using UnityEngine;

/// <summary>This class allows a GameObject with a rigidbody to freeze and unfreeze, not
/// losing the velocity. The velocity is reduced by a constant factor</summary>
[RequireComponent(typeof(Rigidbody))]
public class Rotatable : MonoBehaviour
{
	[SerializeField]
	[Range(0.0f, 0.25f)]
	private float			_adjustmentLength;

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

	private Vector3		GetNearestCase()
	{
		Vector3	localVelocity = transform.parent.InverseTransformDirection(_velocityAtFreeze);
		Vector3	newPosition = transform.localPosition + localVelocity.normalized * _adjustmentLength;

		newPosition.x = Mathf.Round(newPosition.x + 0.5f) - 0.5f;
		newPosition.y = Mathf.Round(newPosition.y + 0.5f) - 0.5f;
		return (newPosition);
	}

	private IEnumerator	MoveToNearestCase(float rotationDuration)
	{
		Vector3	newPosition = GetNearestCase();
		float	distance = Vector3.Distance(newPosition, transform.localPosition);
		float	speed = _velocityAtFreeze.magnitude;
		float	movementDuration = Mathf.Min(rotationDuration, distance / speed);
		
		yield return TransformUtils.LocalMoveInTime(transform, newPosition, movementDuration);
		_placeInGridCoroutine = null;
	}
	
	public void	Freeze(float rotationDuration)
	{
		if (_isFroze)
			return ;
		_isFroze = true;
		_velocityAtFreeze = _rigidbody.linearVelocity;
		_rigidbody.isKinematic = true;
		_placeInGridCoroutine ??= StartCoroutine(MoveToNearestCase(rotationDuration));
	}

	public void	Unfreeze()
	{
		if (!_isFroze)
			return ;
		_isFroze = false;
		_rigidbody.isKinematic = false;
		_rigidbody.linearVelocity = _velocityAtFreeze * _velocityMultiplicatorAtRotation;
	}
}
