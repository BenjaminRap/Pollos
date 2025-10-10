using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Storm : MonoBehaviour {
	[SerializeField] private uint _stepCount = 5;
	[SerializeField] private float _moveDuration = 0.3f;

	private CoroutineHandler _moveCoroutine;
	private Vector3 _goalPosition;
	private Vector3 _move;

	private void Awake() {
		if (!Level.TryGetInstance(out Level level, true))
		{
			Destroy(this);
			return;
		}
		_goalPosition = transform.position;
		Vector3 direction = level.transform.position - transform.position;
		_move = direction / _stepCount;
		_moveCoroutine = new(this);
	}

	public void ComeCloser() {
		_goalPosition += _move;
		_moveCoroutine.Start(TransformUtils.LocalMoveInTime(transform, _goalPosition, _moveDuration), true);
	}

	private void OnTriggerEnter(Collider collider) {
		if (!collider.TryGetComponent(out PollosController characterControler)
			|| !GameManager.TryGetInstance(out GameManager gameManager, true)) {
			return;
		}
		gameManager.Defeat();
		characterControler.Kill();
	}
}
