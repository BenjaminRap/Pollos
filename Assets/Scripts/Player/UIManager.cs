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
    [SerializeField] private Animator _animatorFade= null;
    [SerializeField] private Animator _animatorPlay= null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this._animatorFade.transform.parent);
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
        //_uiCredit.SetActive(false);
        //_playButton.SetActive(true);
        //_quitButton.SetActive(true);
        //_creditButton.SetActive(true);
    }
    public IEnumerator PlayGameCoroutine()
    {
        _animatorPlay.SetBool("PlayClick",true);
        _cancelImage.SetActive(true);
        yield return new WaitForSeconds(8);
        SceneManager.LoadSceneAsync("Benji");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OnClic()
    {
        if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
            audioManager.PlayAudioEffect("Clic", gameObject.transform.position, 2);
    }
    public void PlayGame()
    {
        StartCoroutine(PlayGameCoroutine());
    }
}
