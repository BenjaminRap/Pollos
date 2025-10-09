using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rotatable))]
[RequireComponent(typeof(Rigidbody))]
public class Shock : MonoBehaviour {
	private const float MIN_VELOCITY_DIFF_TO_SHOCK = 8.5f;

	[SerializeField] private UnityEvent<Transform> _onShock;

	private Rigidbody _rigidbody;
	private Rotatable _rotatable;
	private Vector3 _previousVelocity;

	private void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
		_rotatable = GetComponent<Rotatable>();
		_previousVelocity = Vector3.zero;
	}

	private void Update() {
		if (_rotatable.IsFroze)
			return;
		float velocityDiff = _previousVelocity.magnitude - _rigidbody.linearVelocity.magnitude;
		if (velocityDiff > MIN_VELOCITY_DIFF_TO_SHOCK)
			_onShock.Invoke(transform);
		_previousVelocity = _rigidbody.linearVelocity;
	}
}
