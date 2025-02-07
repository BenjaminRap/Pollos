using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager  _instance;

    [SerializeField]
    private List<GameObject>    _levelsPrefabs;

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
}
