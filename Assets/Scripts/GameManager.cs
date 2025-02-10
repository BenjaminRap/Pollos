using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	
	public static bool	TryAndGetInstance(out GameManager gameManager)
	{
		gameManager = _instance;
		if (_instance == null)
		{
			Debug.LogError("The GameManager class has no instance !");
			return (false);
		}
		return (true);
	}
	
	private void	SetLevel(int levelIndex)
	{
		if (levelIndex >= _levelsPrefabs.Count)
		{
			Debug.LogError("SetLevel called with an index too big !");
			return ;
		}
		_levelIndex = levelIndex;
		if (Level.TryAndGetInstance(out Level level))
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
		if (InputManager.TryAndGetInstance(out InputManager inputManager))
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
		if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
			audioManager.playAudioEffect("Victory", gameObject.transform.position, 1);
		if (_levelIndex >= _levelsPrefabs.Count - 1)
		{
            SceneManager.LoadScene(2);
            Debug.Log("No more levels :(");
			return ;
		}
		StartCoroutine(ChangeLevelWithFade(_levelIndex + 1));
		if (InputManager.TryAndGetInstance(out InputManager inputManager))
			inputManager.SetWorldActionsState(false);
	}
	
	public void	Defeat()
	{
		if (_inLevelTransition == true)
			return ;
		_inLevelTransition = true;
		if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
			audioManager.playAudioEffect("Death", gameObject.transform.position, 1);
		StartCoroutine(ChangeLevelAfterDelay(0.001f, _levelIndex));
	}
}
