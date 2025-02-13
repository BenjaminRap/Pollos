using System;
using System.Collections;
using UnityEngine;

/// <summary>This class manages a level : the storms and the rotation of the level.</summary>
public class Level : MonoBehaviour
{
	/// <summary>The angle rotated each input.</summary>
	private const float		_rotationAngle = 90.0f;
	/// <summary>A multiplicator applied on rigidbody's velocity when the level is rotated</summary>
	
	/// <summary>The time it takes for the level to rotate, it shouldn't be greater 
	/// than the rotate animation.</summary>
	[SerializeField]
	private float			_rotationDuration = 0.2f;
	/// <summary>The GameOBject that will be rotated.</summary>
	[SerializeField]
	private Transform		_rotableChild;
	/// <summary>The material that has the storm effect</summary>
	[SerializeField]
	private Material		_stormEffect;
	/// <summary>The curve that describe the alpha of the storm effect over time</summary>
	[SerializeField]
	private AnimationCurve	_alphaCurve;

	private static Level	_instance;
	
	private Coroutine		_rotateCoroutine;
	private Quaternion		_rotationGoal;
	private Storm[]			_storms;
	private float			_averageStormDistanceAtStart;
	private Rotatable[]		_rotatablesObjets;

	private void		Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instance of the level class !");
			Destroy(this);
			return ;
		}
		_instance = this;
		_rotationGoal = _rotableChild.rotation;
		_storms = GetComponentsInChildren<Storm>();
		_averageStormDistanceAtStart = GetAverageStormDistance();
		_rotatablesObjets = GetComponentsInChildren<Rotatable>();
	}
	
	private void	Update()
	{
		float	progression = Mathf.InverseLerp(_averageStormDistanceAtStart, 0, GetAverageStormDistance());
		float	alpha = _alphaCurve.Evaluate(progression);
		_stormEffect.SetFloat("_alphaMultiplicator", alpha);
	}
	
	private void	OnDestroy()
	{
		_stormEffect.SetFloat("_alphaMultiplicator", 0);
	}

	private float	GetAverageStormDistance()
	{
		float	averageStormDistance = 0.0f;

		foreach (Storm storm in _storms)
		{
			averageStormDistance += Vector3.Distance(storm.transform.position, transform.position);
		}
		averageStormDistance /= _storms.Length;
		return (averageStormDistance);
	}
	
	/// <summary>Get the instance of this singleton if there is one.</summary>
	/// <param name="level">This variable will be set to the instance value.</param>
	/// <returns>True if there is an instance, false otherwise.</returns>
	public static bool	TryGetInstance(out Level level)
	{
		level = _instance;
		if (_instance == null)
		{
			Debug.LogError("The class Level has no instance !");
			return (false);
		}
		return (true);
	}
	
	/// <summary>A corotutine that rotates the level and restart the rigidbody's
	/// simulation after that</summary>
	private IEnumerator	RotateLevelToRotationGoal()
	{
		
		yield return TransformUtils.RotateInTime(_rotableChild, _rotationGoal, _rotationDuration);
		UnfreezeRotatables();
		_rotateCoroutine = null;
	}

	/// <summary>Toggle the freezing state of the rotatables objects, freeze
	/// means they won't move with forces.</summary>
	private void	FreezeRotatables()
	{
		foreach (Rotatable rotatable in _rotatablesObjets)
		{
			rotatable.Freeze(_rotationDuration);
		}
	}

	private void	UnfreezeRotatables()
	{
		foreach (Rotatable rotatable in _rotatablesObjets)
		{
			rotatable.Unfreeze();
		}
	}
	
	/// <summary>Place the transform in the nearest grid.</summary>
	public static void	PlaceTransformInGrid(Transform rigidbody)
	{
		Vector3	newPosition; Vector3	position = rigidbody.position;

		newPosition.x = MathF.Round(position.x + 0.5f) - 0.5f;
		newPosition.y = MathF.Round(position.y + 0.5f) - 0.5f;
		newPosition.z = position.z;
		rigidbody.position = newPosition;
	}
	
	/// <summary>Rotate the level.</summary>
	/// <param name="axisValue">The axis of the input : 1 for right and -1 for left.</param>
	public void			Rotate(float axisValue)
	{
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return ;
		characterControler.RotateCharacter();
		foreach (Storm storm in _storms)
		{
			storm.ComeCloser();
		}
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		FreezeRotatables();
		_rotationGoal *= Quaternion.Euler(-axisValue * _rotationAngle * Vector3.forward);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal());
	}
	
	public Transform	GetRotableChild()
	{
		return (_rotableChild);
	}
}
