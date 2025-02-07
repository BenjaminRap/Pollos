using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TransformUtils
{
    public static IEnumerator	RotateAroundInTime(Transform transform, Vector3 axis, float angle, float time)
	{
		float	duration = 0.0f;

		while (duration < time)
		{
			float deltaTime = Time.deltaTime;

			transform.Rotate(axis, angle * deltaTime / time);
			duration += Time.deltaTime;
			yield return (null);
		}
	}
}
