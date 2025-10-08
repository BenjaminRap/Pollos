using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PollosController : MonoBehaviourSingleton<PollosController>
{
	[SerializeField] private Animator	_animPollos;

	protected override void Awake()
	{
		base.Awake();
		_animPollos = GetComponent<Animator>();
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
		transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
	}
}
