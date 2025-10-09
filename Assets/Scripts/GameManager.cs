using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager> {
	private static readonly WaitForSeconds _waitForSeconds0_6 = new(0.6f);
	private static readonly WaitForSeconds _waitForSeconds1 = new(1f);

	[SerializeField] private List<GameObject> _levelsPrefabs;
	[SerializeField] private UIFade _uiFade;

	private int _levelIndex;
	private bool _inLevelTransition;

	protected override void Awake() {
		base.Awake();
		if (_levelsPrefabs.Count == 0) {
			Debug.LogWarning("No levels in the GameManager !");
			return;
		}
		Instantiate(_levelsPrefabs[0]);
	}

	private void SetLevel(int levelIndex) {
		if (levelIndex >= _levelsPrefabs.Count) {
			Debug.LogError("SetLevel called with an index too big !");
			return;
		}
		_levelIndex = levelIndex;
		if (Level.TryGetInstance(out Level level))
			DestroyImmediate(level.gameObject);
		Instantiate(_levelsPrefabs[_levelIndex]);
	}

	private IEnumerator ChangeLevelWithFade(int levelIndex) {
		if (!InputManager.TryGetInstance(out InputManager inputManager))
			yield break;
		inputManager.SetWorldActionsState(false);
		_uiFade.FadeIn();
		yield return _waitForSeconds1;
		SetLevel(levelIndex);
		_uiFade.FadeOut();
		yield return _waitForSeconds0_6;
		inputManager.SetWorldActionsState(true);
		_inLevelTransition = false;
	}

	private IEnumerator ChangeLevelAfterDelay(float delay, int levelIndex) {
		if (!InputManager.TryGetInstance(out InputManager inputManager))
			yield break;
		inputManager.SetWorldActionsState(false);
		yield return new WaitForSeconds(delay);
		SetLevel(levelIndex);
		inputManager.SetWorldActionsState(true);
		_inLevelTransition = false;
	}

	public void Victory() {
		if (_inLevelTransition == true)
			return;
		_inLevelTransition = true;
		if (AudioManager.TryGetInstance(out AudioManager audioManager))
			audioManager.PlayAudioEffect("Victory", gameObject.transform.position, 1);
		if (_levelIndex >= _levelsPrefabs.Count - 1) {
			SceneManager.LoadScene(2);
			Debug.Log("No more levels :(");
			return;
		}
		StartCoroutine(ChangeLevelWithFade(_levelIndex + 1));

	}

	public void Defeat() {
		if (_inLevelTransition == true)
			return;
		_inLevelTransition = true;
		if (AudioManager.TryGetInstance(out AudioManager audioManager))
			audioManager.PlayAudioEffect("Death", gameObject.transform.position, 1);
		StartCoroutine(ChangeLevelAfterDelay(0.5f, _levelIndex));
	}
}
