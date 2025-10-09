using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotatable : MonoBehaviour {
	private const float VELOCITY_MULTIPLICATOR_AT_ROTATION = 0.4f;

	[SerializeField]
	[Range(0.0f, 0.25f)]
	private float _adjustmentLength;

	private Rigidbody _rigidbody;
	private bool _isFroze;
	private Vector3 _velocityAtFreeze;
	private CoroutineHandler _placeInGridCoroutine;

	public bool IsFroze { get => _isFroze; }

	private void Awake() {
		_placeInGridCoroutine = new(this);
		_rigidbody = GetComponent<Rigidbody>();
		_isFroze = false;
		_velocityAtFreeze = Vector3.zero;
	}

	public Vector3 GetNearestGridCell() {
		Vector3 localVelocity = transform.parent.InverseTransformDirection(_velocityAtFreeze);
		Vector3 newPosition = transform.localPosition + localVelocity.normalized * _adjustmentLength;

		newPosition.x = Mathf.Round(newPosition.x + 0.5f) - 0.5f;
		newPosition.y = Mathf.Round(newPosition.y + 0.5f) - 0.5f;
		return newPosition;
	}

	private IEnumerator MoveToNearestGridCell(float rotationDuration) {
		Vector3 newPosition = GetNearestGridCell();
		float distance = Vector3.Distance(newPosition, transform.localPosition);
		float speed = _velocityAtFreeze.magnitude;
		float movementDuration = Mathf.Min(rotationDuration, distance / speed);

		yield return TransformUtils.LocalMoveInTime(transform, newPosition, movementDuration);
	}

	public void Freeze(float rotationDuration) {
		if (_isFroze)
			return;
		_isFroze = true;
		_velocityAtFreeze = _rigidbody.linearVelocity;
		_rigidbody.isKinematic = true;
		_placeInGridCoroutine.Start(MoveToNearestGridCell(rotationDuration), false);
	}

	public void Unfreeze() {
		if (!_isFroze)
			return;
		_isFroze = false;
		_rigidbody.isKinematic = false;
		_rigidbody.linearVelocity = _velocityAtFreeze * VELOCITY_MULTIPLICATOR_AT_ROTATION;
	}
}
