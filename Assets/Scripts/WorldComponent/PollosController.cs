using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PollosController : MonoBehaviour
{
	private static PollosController	_instance;
	

	[SerializeField] private Animator	_animPollos = null;

	private void Start()
	{
		Time.timeScale = 0.1f;
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the PollosController class");
			Destroy(gameObject);
			return ;
		}
		_instance = this;
		_animPollos = GetComponent<Animator>();
	}
	
	public static bool	TryGetInstance(out PollosController characterControler)
	{
		characterControler = _instance;
		if (_instance == null)
		{
			Debug.LogError("PollosController has no instance !");
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
		transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
	}
}
