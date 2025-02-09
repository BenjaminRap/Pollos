using UnityEngine;

public class Draft : MonoBehaviour
{
	[SerializeField]
	private Vector3	_draftDirection;
	[SerializeField]
	private float	_draftLength;
	
	private Vector3	_draftForce;

	private void	Start()
	{
		_draftForce = _draftDirection.normalized * _draftLength;
	}
	
	private void	OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		rigidbody.gravityScale = 0;
		rigidbody.linearVelocity = Vector2.zero;
		Level.placeTransformInGrid(collider.transform);
	}

	private void	OnTriggerExit2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		rigidbody.gravityScale = 1;
		rigidbody.linearVelocity = Vector2.zero;
	}

	private void	OnTriggerStay2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		rigidbody.AddForce(transform.TransformVector(_draftForce));
	}
}
