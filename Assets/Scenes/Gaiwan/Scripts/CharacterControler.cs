using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterControler : MonoBehaviour
{
	private static CharacterControler	_instance;


	[SerializeField] private Animator	_animPollos = null;

	private bool						_isFlying;

	private void Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the CharacterController class");
			Destroy(this);
			return ;
		}
		_instance = this;
		_animPollos = GetComponent<Animator>();
		_animPollos.SetTrigger("TriggerSpawn");

	}
	
	public static CharacterControler	GetInstance()
	{
		return (_instance);
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
	public void RotateCharacter()
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
	
	public void	Kill()
	{
		_animPollos.SetTrigger("TriggerDeath");
	}
	
	public void	ResetRotation()
	{
		transform.rotation = Quaternion.identity;
	}
}
