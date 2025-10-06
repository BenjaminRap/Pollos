using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Goal : MonoBehaviour
{
	private static Goal	_instance;

	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the Goal class !");
			Destroy(gameObject);
			return ;
		}
		_instance = this;
	}
	
	private void	OnTriggerEnter(Collider collider)
	{
		if (!GameManager.TryGetInstance(out GameManager gameManager))
			return ;
        if (collider.CompareTag("Heavy"))
			gameManager.Defeat();
		else if (collider.TryGetComponent(out PollosController _))
			gameManager.Victory();
	}
}
