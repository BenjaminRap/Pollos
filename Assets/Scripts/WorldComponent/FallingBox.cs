using Unity.VisualScripting;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		GameManager	gameManager = GameManager.GetInstance();

		if (gameManager == null)
		{
			Debug.LogError("No instance of the GameManager class !");
			return ;
		}
		if (!other.transform.TryGetComponent<CharacterControler>(out CharacterControler characterControler))
			return ;
		if (other.transform.position.y + 0.5f < transform.position.y)
			gameManager.Defeat();
	}
}
