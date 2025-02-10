using System.Collections;
using UnityEngine;

/// <summary>This class has utils functions that are usefuls for transforms.</summary>
public static class TransformUtils
{
	/// <summary>Rotate the transform from its current rotation to the rotationGoal in time seconds.</summary>
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
		transform.rotation = rotationGoal;
	}
	
	/// <summary>Move the transform from its current position to the positionGoal in time seconds.</summary>
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
		transform.position = positionGoal;
	}
}
