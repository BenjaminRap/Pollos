using UnityEngine;

public class Storm : MonoBehaviour
{
	[SerializeField]
	private float	speed = 1.5f;

	public void	ComeCloser()
	{
		Goal goal = Goal.GetInstance();
		
		if (goal == null)
		{
			Debug.LogError("The goal class has no instance but the ComeClose method if the Storm class has been called !");
			return ;
		}
		Vector3	direction = (goal.transform.position - transform.position).normalized;
		transform.position += direction * speed;
	}
	
	private void	OnTriggerEnter2D(Collider2D col)
	{
		GameManager	gameManager = GameManager.GetInstance();
		
		if (gameManager == null)
		{
			Debug.LogError("The GameManager has no instance !");
			return ;
		}
		gameManager.Defeat();
	}
}
