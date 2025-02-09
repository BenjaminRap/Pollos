using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playButton = null;
    [SerializeField] private GameObject _quitButton = null;
    [SerializeField] private GameObject _creditButton = null;
    [SerializeField] private GameObject _uiCredit= null;
    [SerializeField] private GameObject _cancelImage = null;
    [SerializeField] private GameObject _animatorFade= null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this._animatorFade.gameObject);
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
    public IEnumerator PlayGameCoroutine()
    {
        _cancelImage.SetActive(true);
        yield return new WaitForSeconds(8);
        SceneManager.LoadSceneAsync("Benji");
        //_animatorFade.SetBool("RevealWorld", true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OnClic()
    {
        //Pour les sound effect des clics 
    }
    public void PlayGame()
    {
        StartCoroutine(PlayGameCoroutine());
    }
}
