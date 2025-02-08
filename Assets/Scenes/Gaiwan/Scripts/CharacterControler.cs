using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [SerializeField] private Animator _animPollos = null;
    private Collider2D _collider;
    private bool _isFlying;
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
            _isFlying = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _animPollos.SetBool("BoolIsFlying",true);
            _isFlying = true;
        }
    }
    private void RotateCharacter()
    {
        if (_isFlying == true)
        {
            _animPollos.SetTrigger("TrFlyToRotate");
        }
        if (_isFlying == false)
        {
            _animPollos.SetTrigger("TrIdleToRotate");

        }
    }
}
