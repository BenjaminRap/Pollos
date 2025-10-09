using System.Collections;
using UnityEngine;

public class CoroutineHandler {
	private readonly MonoBehaviour _owner;

	private Coroutine _routine = null;
	private bool _isStoppable = false;

	public CoroutineHandler(MonoBehaviour owner) {
		_owner = owner;
	}

	public bool Start(IEnumerator coroutine, bool isStoppable) {
		if (!Stop())
			return false;
		_isStoppable = isStoppable;
		_routine = _owner.StartCoroutine(Wrap(coroutine));
		return true;
	}

	public bool Stop() {
		if (_routine == null)
			return true;
		if (!_isStoppable)
			return false;
		_owner.StopCoroutine(_routine);
		_routine = null;
		return true;
	}

	private IEnumerator Wrap(IEnumerator coroutine) {
		yield return coroutine;
		_routine = null;
	}

	public bool IsRunning => _routine != null;
}
