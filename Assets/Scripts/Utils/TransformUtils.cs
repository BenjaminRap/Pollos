using System.Collections;
using UnityEngine;

public static class TransformUtils
{
    public static IEnumerator	RotateInTime(Transform transform, Quaternion rotationGoal, float time)
	{
		float		duration = 0.0f;
		Quaternion	rotationStart = transform.rotation;

		while (duration < time)
		{
			transform.rotation = Quaternion.Lerp(rotationStart, rotationGoal, duration / time);
			duration += Time.deltaTime;
			yield return (null);
		}
	}
	
	public static IEnumerator	MoveInTime(Transform transform, Vector3 positionGoal, float time)
	{
		float	duration = 0.0f;
		Vector3	positionStart = transform.position;

		while (duration < time)
		{
			transform.position = Vector3.Lerp(positionStart, positionGoal, duration / time);
			duration += Time.deltaTime;
			yield return (null);
		}
	}
}
