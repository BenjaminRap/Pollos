using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [SerializeField] private Animator _animPollos = null;
    private Collider2D _collider;
    void Start()
    {
        _animPollos = GetComponent<Animator>();
        _animPollos.SetTrigger("TriggerSpawn");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _animPollos.SetBool("BoolIsFlying", false);
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _animPollos.SetBool("BoolIsFlying",true);
        }
    }



}
