using UnityEngine;

/// <summary>This class adds the goal behaviour, when a CharacterController enter
/// its collider, call Victory().</summary>
public class Goal : MonoBehaviour
{
	private static Goal	_instance;

	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the Goal class !");
			Destroy(this);
			return ;
		}
		_instance = this;
	}
	
	/// <summary>When a rigidbody enter the collider : if it has the tag heavy, 
	/// it calls Defeat(). If it has a CharacterController class, it calls
	/// Victory()</summary>
	private void	OnTriggerEnter2D(Collider2D collider)
	{
		if (!GameManager.TryAndGetInstance(out GameManager gameManager))
			return ;
		if (collider.CompareTag("Heavy"))
			gameManager.Defeat();
		else if (collider.TryGetComponent<CharacterControler>(out CharacterControler characterControler))
			gameManager.Victory();
	}
}
