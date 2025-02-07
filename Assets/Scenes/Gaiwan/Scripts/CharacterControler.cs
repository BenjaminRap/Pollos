using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [SerializeField] private Animator _animPollos = null;
    private Collider2D _collider;
    private bool _isGrounded = false;
    void Start()
    {
        _animPollos = GetComponent<Animator>();
        _animPollos.SetTrigger("TriggerSpawn");
    }

    void Update()
    {
        TriggerDeath();
    }
    private void TriggerDeath()
    {
        if (_animPollos != null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _animPollos.SetTrigger("TriggerDeath");
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _animPollos.SetBool("TriggerIsFlying", false);
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            _animPollos.SetBool("TriggerIsFlying",true);
        }
    }



}
