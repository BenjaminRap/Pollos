using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
	private static readonly WaitForSeconds _waitForSeconds8 = new(8);

	[SerializeField] private GameObject _playButton;
	[SerializeField] private GameObject _quitButton;
	[SerializeField] private GameObject _creditButton;
	[SerializeField] private GameObject _uiCredit;
	[SerializeField] private GameObject _cancelImage;
	[SerializeField] private Animator _animatorFade;
	[SerializeField] private Animator _animatorPlay;


	private void Start() {
		DontDestroyOnLoad(_animatorFade.transform.parent);
	}

	public void OnCreditClic() {
		_uiCredit.SetActive(true);
		_playButton.SetActive(false);
		_quitButton.SetActive(false);
		_creditButton.SetActive(false);
	}

	public void OnExitCreditClic() {
		//_uiCredit.SetActive(false);
		//_playButton.SetActive(true);
		//_quitButton.SetActive(true);
		//_creditButton.SetActive(true);
	}

	public IEnumerator PlayGameCoroutine() {
		_animatorPlay.SetBool("PlayClick", true);
		_cancelImage.SetActive(true);
		yield return _waitForSeconds8;
		SceneManager.LoadSceneAsync("InGame");
	}

	public void ExitGame() {
		Application.Quit();
	}

	public void OnClic() {
		if (AudioManager.TryGetInstance(out AudioManager audioManager, true))
			audioManager.PlayAudioEffect("Clic", gameObject.transform.position, 2);
	}

	public void PlayGame() {
		StartCoroutine(PlayGameCoroutine());
	}
}
