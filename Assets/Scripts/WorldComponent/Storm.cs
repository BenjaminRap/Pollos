using UnityEngine;

public class Storm : MonoBehaviour
{
	[SerializeField]
	private uint		_stepCount = 5;
	[SerializeField]
	private float		_moveDuration = 0.3f;
	
	private Coroutine	_moveCoroutine;
	private Vector3		_goalPosition;
	private Vector3		_move;
	
	private void	Start()
	{
		Level	level = Level.GetInstance();

		if (level == null)
		{
			Debug.LogError("The class Level has no instance !");
			Destroy(this);
			return ;
		}
		_goalPosition = transform.position;
		Vector3	direction = level.transform.position - transform.position;
		_move = direction / _stepCount;
	}

	public void		ComeCloser()
	{
		_goalPosition += _move;
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
		CharacterControler	characterControler = CharacterControler.GetInstance();

		if (characterControler == null)
		{
			Debug.LogError("The class CharacterController has no instance !");
			return ;
		}
		gameManager.Defeat();
		characterControler.Kill();
	}
}
