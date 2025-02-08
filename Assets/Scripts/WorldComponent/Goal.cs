using UnityEngine;

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
	
	private void	OnTriggerEnter2D(Collider2D collider)
	{
		GameManager	gameManager = GameManager.GetInstance();
		
		if (gameManager == null)
		{
			Debug.LogError("The GameManager has no instance !");
			return ;
		}

		if (collider.CompareTag("Heavy"))
			gameManager.Defeat();
		else if (collider.TryGetComponent<CharacterControler>(out CharacterControler characterControler))
			gameManager.Victory();
	}

	public static Goal	GetInstance()
	{
		return (_instance);
	}
}
