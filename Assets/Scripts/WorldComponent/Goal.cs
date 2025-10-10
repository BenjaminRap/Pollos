using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Goal : MonoBehaviourSingleton<Goal> {
	private void OnTriggerEnter(Collider collider) {
		if (!GameManager.TryGetInstance(out GameManager gameManager, true))
			return;
		if (collider.CompareTag("Heavy"))
			gameManager.Defeat();
		else if (collider.TryGetComponent(out PollosController _))
			gameManager.Victory();
	}
}
