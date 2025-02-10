using Unity.VisualScripting;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!GameManager.TryAndGetInstance(out GameManager gameManager))
			return ;
		if (!other.transform.TryGetComponent<CharacterControler>(out CharacterControler characterControler))
			return ;
		if (other.transform.position.y + 0.5f < transform.position.y)
			gameManager.Defeat();
	}
}
