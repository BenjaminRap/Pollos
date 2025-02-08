using UnityEngine;

public class MoveSky : MonoBehaviour
{
	[SerializeField]
	private Vector2	_minMaxOffset;
	[SerializeField]
	private float	_maxSpeed;

	private Vector3	_direction;

	private void	Start()
	{
		if (_minMaxOffset.x > _minMaxOffset.y)
		{
			Debug.LogError("The min value is superior to the max value in the MoveSky script !");
			Destroy(this);
			return ;
		}
		_direction = Random.Range(-1.0f, 1.0f) * _maxSpeed * Vector3.right;
	}
	
	private void	Update()
	{
		transform.localPosition += _direction * Time.deltaTime;			
		if (transform.localPosition.x < _minMaxOffset.x)
		{
			transform.localPosition = new Vector3(_minMaxOffset.x, transform.localPosition.y, transform.localPosition.z);
			_direction *= -1;
		}
		else if (transform.localPosition.x > _minMaxOffset.y)
		{
			transform.localPosition = new Vector3(_minMaxOffset.y, transform.localPosition.y, transform.localPosition.z);
			_direction *= -1;
		}
	}
}
