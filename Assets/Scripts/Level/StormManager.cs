using UnityEngine;

/// <summary>This class is a singleton that manages the storms, it destroy itslef
/// if there is no storms. It moves the storms and update the storm shader alpha
/// multiplicator.</summary>
public class StormManager : MonoBehaviour
{
	private static StormManager	_instance;

	/// <summary>The material that has the storm effect</summary>
	[SerializeField]
	private Material			_stormEffect;
	/// <summary>The curve that describe the alpha of the storm effect over time</summary>
	[SerializeField]
	private AnimationCurve		_alphaCurve;

	private Storm[]				_storms;
	private float				_averageStormDistanceAtStart;
	
	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the StormManager class !");
			Destroy(gameObject);
			return ;
		}
		_instance = this;
		_storms = GetComponentsInChildren<Storm>();
		if (_storms.Length == 0)
		{
			Debug.LogWarning("No storm on this level, quitting");
			Destroy(gameObject);
			return ;
		}
		_averageStormDistanceAtStart = GetAverageStormDistance();
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
	
	public void	ComeCloser()
	{
		foreach (Storm storm in _storms)
		{
			storm.ComeCloser();
		}
	}
}
