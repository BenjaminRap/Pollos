using UnityEngine;

[RequireComponent(typeof(StormManager))]
public class Level : MonoBehaviourSingleton<Level> {
	private StormManager _stormManager;
	private RotationManager _rotationManager;

	protected override void Awake() {
		base.Awake();
		_stormManager = GetComponent<StormManager>();
	}

	private void Start() {
		RotationManager.TryGetInstance(out _rotationManager);
	}

	public void RotateFace(int axisValue) {
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return;
		characterControler.RotateCharacter();
		_stormManager.ComeCloser();
		_rotationManager.RotateFace(axisValue);
	}

	public void RotateCube(Vector2Int axisValue) {
		if (axisValue.x != 0 && axisValue.y != 0)
			return;
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return;
		characterControler.RotateCharacter();
		_rotationManager.RotateCube(axisValue);
	}
}
