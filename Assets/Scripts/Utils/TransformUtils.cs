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
	public static IEnumerator	LocalMoveInTime(Transform transform, Vector3 localPositionGoal, float time)
	{
		float	duration = 0.0f;
		Vector3	positionStart = transform.localPosition;

		while (duration < time)
		{
			transform.localPosition = Vector3.Lerp(positionStart, localPositionGoal, duration / time);
			duration += Time.deltaTime;
			yield return (null);
		}
		transform.localPosition = localPositionGoal;;
	}
	
	/// <summary>Return a new vector with the same x and y as vec but with value as z.</summary>
	public static Vector3	SetZ(Vector3	vec, float value)
	{
		return (new(vec.x, vec.y, value));
	}
}
