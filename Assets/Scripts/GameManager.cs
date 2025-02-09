using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager	_instance;

	[SerializeField]
	private List<GameObject>	_levelsPrefabs;
	private int					_levelIndex;
	private bool				_inLevelTransition;
	[SerializeField] 
	private UIFade _uiFade = null;

	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the GameManager class");
			Destroy(this);
			return ;
		}
		_instance = this;
		if (_levelsPrefabs.Count == 0)
		{
			Debug.LogWarning("No levels in the GameManager !");
			return ;
		}
		Instantiate(_levelsPrefabs[0]);
	}
	
	public static GameManager	GetInstance()
	{
		return (_instance);
	}
	
	private void	SetLevel(int levelIndex)
	{
		if (levelIndex >= _levelsPrefabs.Count)
		{
			Debug.LogError("SetLevel called with an index too big !");
			return ;
		}
		_levelIndex = levelIndex;
		Level level = Level.GetInstance();
		
		if (level != null)
			DestroyImmediate(level.gameObject);
		Instantiate(_levelsPrefabs[_levelIndex]);
	}
	
	private IEnumerator	ChangeLevelWithFade(int levelIndex)
	{
		_uiFade.FadeIn();
		yield return (new WaitForSeconds(1f));
		SetLevel(levelIndex);
        _uiFade.FadeOut();
		yield return (new WaitForSeconds(0.6f));
		InputManager inputManager = InputManager.GetInstance();
		if (inputManager != null)
			inputManager.SetWorldActionsState(true);
        _inLevelTransition = false;
    }
	
	private IEnumerator	ChangeLevelAfterDelay(float delay, int levelIndex)
	{
		yield return (new WaitForSeconds(delay));
		SetLevel(levelIndex);
		_inLevelTransition = false;
	}
	
	public void	Victory()
	{
		if (_inLevelTransition == true)
			return ;
		_inLevelTransition = true;
		AudioManager	audioManager = AudioManager.GetInstance();
		if (audioManager != null)
			audioManager.playAudioEffect("victory", gameObject.transform.position, 1);
		if (_levelIndex >= _levelsPrefabs.Count - 1)
		{
			Debug.Log("No more levels :(");
			return ;
		}
		StartCoroutine(ChangeLevelWithFade(_levelIndex + 1));
		InputManager inputManager = InputManager.GetInstance();
		if (inputManager != null)
			inputManager.SetWorldActionsState(false);
	}
	
	public void	Defeat()
	{
		if (_inLevelTransition == true)
			return ;
		_inLevelTransition = true;
		AudioManager	audioManager = AudioManager.GetInstance();
		if (audioManager != null)
			audioManager.playAudioEffect("defeat", gameObject.transform.position, 1);
		StartCoroutine(ChangeLevelAfterDelay(0.001f, _levelIndex));
	}
}
