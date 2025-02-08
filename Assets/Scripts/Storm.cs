using UnityEngine;

public class Storm : MonoBehaviour
{
	[SerializeField]
	private float		_speed = 1.5f;
	[SerializeField]
	private float		_moveDuration = 0.3f;
	
	private Coroutine	_moveCoroutine;
	private Vector3		_goalPosition;
	
	private void	Start()
	{
		_goalPosition = transform.position;
	}

	public void		ComeCloser()
	{
		Goal goal = Goal.GetInstance();
		
		if (goal == null)
		{
			Debug.LogError("The goal class has no instance but the ComeClose method if the Storm class has been called !");
			return ;
		}
		Vector3	direction = (goal.transform.position - transform.position).normalized;
		_goalPosition += direction * _speed;
		if (_moveCoroutine != null)
			StopCoroutine(_moveCoroutine);
		_moveCoroutine = StartCoroutine(TransformUtils.MoveInTime(transform, _goalPosition, _moveDuration));
	}
	
	private void	OnTriggerEnter2D(Collider2D col)
	{
		GameManager	gameManager = GameManager.GetInstance();
		
		if (gameManager == null)
		{
			Debug.LogError("The GameManager has no instance !");
			return ;
		}
		gameManager.Defeat();
	}
}
