using System.Collections;
using UnityEngine;

/// <summary>This class allows a GameObject with a rigidbody to freeze and unfreeze, not
/// losing the velocity. The velocity is reduced by a constant factor</summary>
[RequireComponent(typeof(Rigidbody))]
public class Rotatable : MonoBehaviour
{
	private const float		_velocityMultiplicatorAtRotation = 0.4f;
	private Rigidbody		_rigidbody;
	private bool			_isFroze;
	private Vector3			_velocityAtFreeze;
	private Coroutine		_placeInGridCoroutine;
	
	public bool				IsFroze { get => _isFroze; }

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_isFroze = false;
		_velocityAtFreeze = Vector3.zero;
	}

	private Vector3		GetNearestCase()
	{
		Vector3	newPosition;

		newPosition.x = Mathf.Round(transform.localPosition.x + 0.5f) - 0.5f;
		newPosition.y = Mathf.Round(transform.localPosition.y + 0.5f) - 0.5f;
		newPosition.z = transform.localPosition.z;
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

	public void	ToggleFreeze(float rotationDuration)
	{
		_isFroze = !_isFroze;
		if (_isFroze)
		{
			_velocityAtFreeze = _rigidbody.linearVelocity;
			_rigidbody.isKinematic = true;
			_placeInGridCoroutine ??= StartCoroutine(MoveToNearestCase(rotationDuration));
		}
		else
		{
			_rigidbody.isKinematic = false;
			_rigidbody.linearVelocity = _velocityAtFreeze * _velocityMultiplicatorAtRotation;
		}
	}
}
