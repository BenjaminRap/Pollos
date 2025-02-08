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

	private void	OnTriggerStay2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		rigidbody.AddForce(transform.TransformVector(_draftForce));
	}
}
