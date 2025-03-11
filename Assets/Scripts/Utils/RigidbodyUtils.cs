using UnityEngine;

public static class RigidbodyUtils
{
	public static bool	CanRigidbodyMoveTo(Rigidbody rigidbody, Vector3 position)
	{
		Vector3	direction = position - rigidbody.transform.position;
		return (!rigidbody.SweepTest(direction.normalized, out RaycastHit _, direction.magnitude, QueryTriggerInteraction.Ignore));
	}
}
