using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControler : MonoBehaviour
{
	private static CharacterControler	_instance;
	
	private const float					_velocityMultiplicatorAtRotation = 0.4f;


	[SerializeField] private Animator	_animPollos = null;

	private bool						_isFlying;
	private Rigidbody2D					_rigidbody;

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
		_rigidbody = GetComponent<Rigidbody2D>();
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
		_rigidbody.linearVelocity *= _velocityMultiplicatorAtRotation;
		if (_isFlying == true)
		{
			_animPollos.SetTrigger("TrFlyToRotate");
		}
		else
		{
			_animPollos.SetTrigger("TrIdleToRotate");

		}
	}
	
	public void	StopSimulation()
	{
		_rigidbody.simulated = false;
	}

	public void	RestartSimulation()
	{
		_rigidbody.simulated = true;
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
