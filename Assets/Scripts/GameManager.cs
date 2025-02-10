using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>This class is a singleton that manages the victory and defeat,
/// by resetting the level or going to the next one. It changes the scene to the
/// end scene if there is no remaining level.</summary>
public class GameManager : MonoBehaviour
{
	private static GameManager	_instance;

	[SerializeField]
	private List<GameObject>	_levelsPrefabs;
	private int					_levelIndex;
	/// <summary>True if we are transitioning between two levels.</summary>
	private bool				_inLevelTransition;
	[SerializeField] 
	private UIFade 				_uiFade;

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
	
	/// <summary>Get the instance of this singleton if there is one.</summary>
	/// <param name="gameManager">This variable will be set to the instance value.</param>
	/// <returns>True if there is an instance, false otherwise.</returns>
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
	
	/// <summary>Change the level to those at levelIndex.</summary>
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
	
	/// <summary>Set the transition between two levels and change the level.
	/// It deactivate the input at start and reactivate it at end.</summary>
	/// <param name="levelIndex"></param>
	private IEnumerator	ChangeLevelWithFade(int levelIndex)
	{
		if (!InputManager.TryAndGetInstance(out InputManager inputManager))
			yield break ;
		inputManager.SetWorldActionsState(false);
		_uiFade.FadeIn();
		yield return (new WaitForSeconds(1f));
		SetLevel(levelIndex);
        _uiFade.FadeOut();
		yield return (new WaitForSeconds(0.6f));
		inputManager.SetWorldActionsState(true);
        _inLevelTransition = false;
    }
	
	/// <summary>Change the level to the one at levelIndex after a delay.
	/// It deactivate the input at start and reactivate it at end.</summary>
	private IEnumerator	ChangeLevelAfterDelay(float delay, int levelIndex)
	{
		if (!InputManager.TryAndGetInstance(out InputManager inputManager))
			yield break ;
		inputManager.SetWorldActionsState(false);
		yield return (new WaitForSeconds(delay));
		SetLevel(levelIndex);
		inputManager.SetWorldActionsState(true);
		_inLevelTransition = false;
	}
	
	/// <summary>Add a victory sound and set the level to the next</summary>
	public void	Victory()
	{
		if (_inLevelTransition == true)
			return ;
		_inLevelTransition = true;
		if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
			audioManager.PlayAudioEffect("Victory", gameObject.transform.position, 1);
		if (_levelIndex >= _levelsPrefabs.Count - 1)
		{
            SceneManager.LoadScene(2);
            Debug.Log("No more levels :(");
			return ;
		}
		StartCoroutine(ChangeLevelWithFade(_levelIndex + 1));

	}
	
	/// <summary>Add a defeat sound and reset the level</summary>
	public void	Defeat()
	{
		if (_inLevelTransition == true)
			return ;
		_inLevelTransition = true;
		if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
			audioManager.PlayAudioEffect("Death", gameObject.transform.position, 1);
		StartCoroutine(ChangeLevelAfterDelay(0.5f, _levelIndex));
	}
}
