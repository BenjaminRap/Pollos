using UnityEngine;

public class SetActivePop : MonoBehaviour
{
    [SerializeField] private GameObject[] _popText = null;
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
        //SceneManager.LoadSceneAsync("");
        foreach (GameObject pop in _popText)
        {
            pop.SetActive(true);
        }
    }
}
