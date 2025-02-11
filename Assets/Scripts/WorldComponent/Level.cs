using System;
using System.Collections;
using UnityEngine;

/// <summary>This class manages a level : the storms and the rotation of the level.</summary>
public class Level : MonoBehaviour
{
	/// <summary>The angle rotated each input.</summary>
	private const float		_rotationAngle = 90.0f;
	/// <summary>A multiplicator applied on rigidbody's velocity when the level is rotated</summary>
	private const float		_velocityMultiplicatorAtRotation = 0.4f;
	
	/// <summary>The time it takes for the level to rotate, it shouldn't be greater 
	/// than the rotate animation.</summary>
	[SerializeField]
	private float			_rotationDuration = 0.2f;
	/// <summary>The GameOBject that will be rotated.</summary>
	[SerializeField]
	private GameObject		_rotableChild;
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
	private Rigidbody2D[]	_rigidBodys;

	private void		Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instance of the level class !");
			Destroy(this);
			return ;
		}
		_instance = this;
		_rotationGoal = _rotableChild.transform.rotation;
		_storms = GetComponentsInChildren<Storm>();
		_averageStormDistanceAtStart = GetAverageStormDistance();
		_rigidBodys = GetComponentsInChildren<Rigidbody2D>();
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
	public static bool	TryAndGetInstance(out Level level)
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
	// simulation after that</summary>
	private IEnumerator	RotateLevelToRotationGoal()
	{
		
		yield return TransformUtils.RotateInTime(_rotableChild.transform, _rotationGoal, _rotationDuration);
		StartRigidbodysSimulation();
		_rotateCoroutine = null;
	}
	
	/// <summary>Stop all the rigidbody's simulation and place them in the nearest case.</summary>
	private  void	StopRigidbodysSimulationInGrid()
	{
		foreach (Rigidbody2D rigidbody in _rigidBodys)
		{
			Level.PlaceTransformInGrid(rigidbody.transform);
			rigidbody.linearVelocity *= _velocityMultiplicatorAtRotation;
			rigidbody.bodyType = RigidbodyType2D.Kinematic;
		}
	}

	/// <summary>Restart the rigidbody's simulation.</summary>
	private void	StartRigidbodysSimulation()
	{
		foreach (Rigidbody2D rigidbody in _rigidBodys)
		{
			rigidbody.bodyType = RigidbodyType2D.Dynamic;
		}
	}
	
	/// <summary>Place the transform in the nearest grid.</summary>
	public static void	PlaceTransformInGrid(Transform rigidbody)
	{
		Vector3	newPosition;
		Vector3	position = rigidbody.position;

		newPosition.x = MathF.Round(position.x + 0.5f) - 0.5f;
		newPosition.y = MathF.Round(position.y + 0.5f) - 0.5f;
		newPosition.z = position.z;
		rigidbody.position = newPosition;
	}
	
	/// <summary>Rotate the level.</summary>
	/// <param name="axisValue">The axis of the input : 1 for right and -1 for left.</param>
	public void			Rotate(float axisValue)
	{
		if (!CharacterControler.TryAndGetInstance(out CharacterControler characterControler))
			return ;
		characterControler.RotateCharacter();
		foreach (Storm storm in _storms)
		{
			storm.ComeCloser();
		}
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		StopRigidbodysSimulationInGrid();
		_rotationGoal *= Quaternion.Euler(-axisValue * _rotationAngle * Vector3.forward);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal());
	}
}
