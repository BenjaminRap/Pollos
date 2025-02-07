using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [SerializeField] private Animator _animPoilos = null;
    void Start()
    {
        _animPoilos = GetComponent<Animator>();
    }

    void Update()
    {
        TriggerDeath();
    }
    private void TriggerDeath()
    {
        if (_animPoilos != null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _animPoilos.SetTrigger("TriggerDeath");
            }
        }
    }

}
