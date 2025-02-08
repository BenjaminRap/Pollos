using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Shock : MonoBehaviour
{
	private const float					_minVelocityDiffToShock = 8.5f;

	private Rigidbody2D					_rigidbody;
	private Vector2						_previousVelocity;
	
	void	Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_previousVelocity = Vector2.zero;
	}

	private void	Update()
	{
		float velocityDiff = _previousVelocity.magnitude - _rigidbody.linearVelocity.magnitude;
		if (velocityDiff > _minVelocityDiffToShock)
			Debug.Log(velocityDiff);
		_previousVelocity = _rigidbody.linearVelocity;
	}
}
