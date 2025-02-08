using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager  _instance;

    [SerializeField]
    private List<GameObject>    _levelsPrefabs;
    private int                 _levelIndex;

    private void    Start()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiples instances of the GameManager class");
            Destroy(this);
            return ;
        }
        _instance = this;
        if (_levelsPrefabs.Count == 0)
        {
            Debug.LogWarning("No levels in the GameManager !");
            return ;
        }
        Instantiate(_levelsPrefabs[0]);
    }
    
    public static GameManager   GetInstance()
    {
        return (_instance);
    }
    
    private void    SetLevel(int levelIndex)
    {
        if (levelIndex >= _levelsPrefabs.Count)
        {
            Debug.LogError("SetLevel called with an index too big !");
            return ;
        }
        _levelIndex = levelIndex;
        Level level = Level.GetInstance();
        
        if (level != null)
            DestroyImmediate(level.gameObject);
        Instantiate(_levelsPrefabs[_levelIndex]);
    }
    
    private IEnumerator GoToNextLevelCoroutine()
    {
        Debug.Log("Victory !!!");
        yield return (new WaitForSeconds(2f));
        SetLevel(_levelIndex + 1);
    }
    
    public void     GoToNextLevel()
    {
        if (_levelIndex >= _levelsPrefabs.Count - 1)
        {
            Debug.Log("No more levels :(");
            return ;
        }
        StartCoroutine(GoToNextLevelCoroutine());
    }
}
