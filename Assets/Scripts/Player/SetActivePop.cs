using UnityEngine;

public class SetActivePop : MonoBehaviour
{
    [SerializeField] private GameObject _fade = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame()
    {
        _fade.SetActive(true);
    }
}
