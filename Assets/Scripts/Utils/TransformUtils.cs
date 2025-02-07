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
}
