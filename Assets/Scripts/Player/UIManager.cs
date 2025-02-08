using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playButton = null;
    [SerializeField] private GameObject _quitButton = null;
    [SerializeField] private GameObject _creditButton = null;
    [SerializeField] private GameObject _uiCredit= null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCreditClic()
    {
        _uiCredit.SetActive(true);
        _playButton.SetActive(false);
        _quitButton.SetActive(false);
        _creditButton.SetActive(false);
    }
    public void OnExitCreditClic()
    {
        _uiCredit.SetActive(false);
        _playButton.SetActive(true);
        _quitButton.SetActive(true);
        _creditButton.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToLobby()
    {
        SceneManager.LoadSceneAsync("");
    }
    public void OnClic()
    {
        //Pour les sound effect des clics 
    }

}
