using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterControler : MonoBehaviour
{
	private static CharacterControler	_instance;
	


	[SerializeField] private Animator	_animPollos = null;

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
	}
	
	public static bool	TryAndGetInstance(out CharacterControler characterControler)
	{
		characterControler = _instance;
		if (_instance == null)
		{
			Debug.LogError("CharacterController has no instance !");
			return (false);
		}
		return (true);
	}

	private void OnCollisionEnter(Collision other)
	{
		_animPollos.SetBool("BoolIsFlying", false);
	}
	private void OnCollisionExit(Collision other)
	{
		_animPollos.SetBool("BoolIsFlying", true);
	}
	public void RotateCharacter()
	{
		_animPollos.SetTrigger("TriggerRotate");
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
