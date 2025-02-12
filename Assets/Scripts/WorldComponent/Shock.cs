using UnityEngine;
using UnityEngine.Events;

/// <summary>This class adds a shock event when the rigidbody stop his velocity brutally.</summary>
[RequireComponent(typeof(Rigidbody))]
public class Shock : MonoBehaviour
{
	/// <summary>The minimum difference of velocity between two frame for the
	/// event to be called.</summary>
	private const float				_minVelocityDiffToShock = 8.5f;
	
	/// <summary>The event that is called on shock. It should take a GameObject has
	/// his unique parameter.</summary>
	[SerializeField]
	private UnityEvent<Transform>	_onShock;

	private Rigidbody				_rigidbody;
	private Vector3					_previousVelocity;
	
	void	Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_previousVelocity = Vector3.zero;
	}

	private void	Update()
	{
		float velocityDiff = _previousVelocity.magnitude - _rigidbody.linearVelocity.magnitude;
		if (velocityDiff > _minVelocityDiffToShock)
			_onShock.Invoke(transform);
		_previousVelocity = _rigidbody.linearVelocity;
	}
}
