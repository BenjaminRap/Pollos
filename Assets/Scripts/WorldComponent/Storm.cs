using UnityEngine;

/// <summary>The class that manages a single storm cloud.</summary>
[RequireComponent(typeof(Collider))]
public class Storm : MonoBehaviour
{
	/// <summary>The number of rotate the player can make before the storm
	/// has finished is movement to the center of the map.</summary>
	[SerializeField]
	private uint		_stepCount = 5;
	/// <summary>The duration of the movement from a step to another.</summary>
	[SerializeField]
	private float		_moveDuration = 0.3f;
	
	private Coroutine	_moveCoroutine;
	private Vector3		_goalPosition;
	private Vector3		_move;
	
	private void	Start()
	{
		if (!Level.TryGetInstance(out Level level))
			return ;
		_goalPosition = transform.position;
		Vector3	direction = level.transform.position - transform.position;
		_move = direction / _stepCount;
	}

	/// <summary>Move forward to the center of the level, from 1 step.</summary>
	public void		ComeCloser()
	{
		_goalPosition += _move;
		if (_moveCoroutine != null)
			StopCoroutine(_moveCoroutine);
		_moveCoroutine = StartCoroutine(TransformUtils.LocalMoveInTime(transform, _goalPosition, _moveDuration));
	}
	
	/// <summary>When the CharacterController enter this collider, this functions
	/// calls the Defeat() functions.</summary>
	/// <param name="collider"></param>
	private void	OnTriggerEnter(Collider collider)
	{
		if (!collider.TryGetComponent<PollosController>(out PollosController characterControler)
			|| !GameManager.TryGetInstance(out GameManager gameManager))
		{
			return ;
		}
		gameManager.Defeat();
		characterControler.Kill();
	}
}
