using System;
using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
	private const float		_rotationAngle = 90.0f;
	private const float		_velocityMultiplicatorAtRotation = 0.4f;
	
	[SerializeField]
	private float			_rotationDuration = 0.2f;
	[SerializeField]
	private GameObject		_rotableChild;
	[SerializeField]
	private Material		_stormEffect;
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
	
	public static Level	GetInstance()
	{
		return (_instance);
	}
	
	private IEnumerator	RotateLevelToRotationGoal(CharacterControler characterController)
	{
		yield return StartCoroutine(TransformUtils.RotateInTime(_rotableChild.transform, _rotationGoal, _rotationDuration));
		StartRigidbodysSimulation();
	}
	
	private  void	StopRigidbodysSimulationInGrid()
	{
		foreach (Rigidbody2D rigidbody in _rigidBodys)
		{
			Vector3	newPosition;
			Vector3	position = rigidbody.transform.position;

			newPosition.x = MathF.Round(position.x + 0.5f) - 0.5f;
			newPosition.y = MathF.Round(position.y + 0.5f) - 0.5f;
			newPosition.z = position.z;
			rigidbody.transform.position = newPosition;
			rigidbody.linearVelocity *= _velocityMultiplicatorAtRotation;
			rigidbody.simulated = false;
		}
	}

	private void	StartRigidbodysSimulation()
	{
		foreach (Rigidbody2D rigidbody in _rigidBodys)
		{
			rigidbody.simulated = true;
		}
	}
	
	public void			Rotate(float axisValue)
	{
		CharacterControler	characterControler = CharacterControler.GetInstance();

		if (characterControler == null)
		{
			Debug.LogError("The CharacterControler class has no instance !");
			return ;
		}
		characterControler.RotateCharacter();
		foreach (Storm storm in _storms)
		{
			storm.ComeCloser();
		}
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		StopRigidbodysSimulationInGrid();
		_rotationGoal *= Quaternion.Euler(-axisValue * _rotationAngle * Vector3.forward);
		_rotateCoroutine = StartCoroutine(RotateLevelToRotationGoal(characterControler));
	}
}
