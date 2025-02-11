using UnityEngine.SceneManagement;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    [SerializeField] private GameObject _pauseUI = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClosePauseGame()
    {
        _pauseUI.SetActive(false);
    }
    public void BackToLobby()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
