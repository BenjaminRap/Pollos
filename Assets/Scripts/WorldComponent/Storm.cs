using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Storm : MonoBehaviour
{
	[SerializeField] private uint	_stepCount = 5;
	[SerializeField] private float	_moveDuration = 0.3f;
	
	private Coroutine	_moveCoroutine;
	private Vector3	_goalPosition;
	private Vector3	_move;
	
	private void	Awake()
	{
		if (!Level.TryGetInstance(out Level level))
			return ;
		_goalPosition = transform.position;
		Vector3	direction = level.transform.position - transform.position;
		_move = direction / _stepCount;
	}

	public void	ComeCloser()
	{
		_goalPosition += _move;
		if (_moveCoroutine != null)
			StopCoroutine(_moveCoroutine);
		_moveCoroutine = StartCoroutine(TransformUtils.LocalMoveInTime(transform, _goalPosition, _moveDuration));
	}
	
	private void	OnTriggerEnter(Collider collider)
	{
		if (!collider.TryGetComponent(out PollosController characterControler)
			|| !GameManager.TryGetInstance(out GameManager gameManager))
		{
			return ;
		}
		gameManager.Defeat();
		characterControler.Kill();
	}
}
