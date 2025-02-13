using UnityEngine;

/// <summary>This class allows a GameObject wit ha rigidbody to freeze and unfreeze, not
/// losing the velocity. The velocity is reduced by a constant factor</summary>
[RequireComponent(typeof(Rigidbody))]
public class Rotatable : MonoBehaviour
{
	private const float		_velocityMultiplicatorAtRotation = 0.4f;
	private Rigidbody		_rigidbody;
	private Vector3			_velocityAtFreeze;
	private bool			_isFroze;
	
	public bool				IsFroze { get => _isFroze; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
		_velocityAtFreeze = Vector3.zero;
		_isFroze = false;
    }

    private void	PlaceInGrid()
	{
		Vector3	newPosition;

		newPosition.x = Mathf.Round(transform.position.x + 0.5f) - 0.5f;
		newPosition.y = Mathf.Round(transform.position.y + 0.5f) - 0.5f;
		newPosition.z = transform.position.z;
		transform.position = newPosition;
	}

	public void	ToggleFreeze()
	{
		_isFroze = !_isFroze;
		if (_isFroze)
		{
			PlaceInGrid();
			_velocityAtFreeze = _rigidbody.linearVelocity;
			_rigidbody.isKinematic = true;
		}
		else
		{
			_rigidbody.isKinematic = false;
			_rigidbody.linearVelocity = _velocityAtFreeze * _velocityMultiplicatorAtRotation;
		}
	}
}
