using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour {
	public void BTLClick() {
		SceneManager.LoadSceneAsync("MainMenu");
	}
}
