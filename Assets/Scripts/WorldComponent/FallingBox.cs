using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FallingBox : MonoBehaviour {
	private void OnCollisionEnter(Collision other) {
		if (!GameManager.TryGetInstance(out GameManager gameManager))
			return;
		if (!other.transform.TryGetComponent(out PollosController _))
			return;
		if (other.transform.position.y + 0.5f < transform.position.y)
			gameManager.Defeat();
	}
}
