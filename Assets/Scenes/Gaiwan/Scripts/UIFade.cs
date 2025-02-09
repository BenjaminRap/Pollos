using UnityEngine;

public class UIFade : MonoBehaviour
{
    [SerializeField] private Animator _animatorFade = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animatorFade.SetBool("RevealWorld", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeIn()
    {
        _animatorFade.SetBool("RevealWorld", false);
    }
    public void FadeOut()
    {
        _animatorFade.SetBool("RevealWorld", true);
    }
}
