using UnityEngine.SceneManagement;
using UnityEngine;

public class UIPause : MonoBehaviour {
	[SerializeField] private GameObject _pauseUI = null;

	public void ClosePauseGame() {
		_pauseUI.SetActive(false);
	}

	public void BackToLobby() {
		SceneManager.LoadSceneAsync("MainMenu");
	}
}
