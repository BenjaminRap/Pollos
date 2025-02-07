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
	
	private void	OnTriggerEnter2D(Collider2D col)
	{
		GameManager	gameManager = GameManager.GetInstance();
		
		if (gameManager == null)
		{
			Debug.LogError("The GameManager has no instance !");
			return ;
		}
		gameManager.GoToNextLevel();
	}
}
