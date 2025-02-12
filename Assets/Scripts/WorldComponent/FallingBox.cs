using UnityEngine;

/// <summary>This class adds the behaviour of destroying rigidbodys when falling 
/// on them.</summary>
[RequireComponent(typeof(Collider))]
public class FallingBox : MonoBehaviour
{
	/// <summary>When this GameObject falls on a rigidbody with the characterController
	/// monobehaviour (the pollos), it kills the pollos.</summary>
	/// <param name="other"></param>
	private void OnCollisionEnter(Collision other)
	{
		if (!GameManager.TryAndGetInstance(out GameManager gameManager))
			return ;
		if (!other.transform.TryGetComponent<CharacterControler>(out CharacterControler characterControler))
			return ;
		if (other.transform.position.y + 0.5f < transform.position.y)
			gameManager.Defeat();
	}
}
