using System.Linq;
using UnityEngine;

public class StormManager : MonoBehaviourSingleton<StormManager>
{
	[SerializeField] private Material	_stormEffect;
	[SerializeField] private AnimationCurve	_alphaCurve;

	private Storm[]	_storms;
	private float	_averageStormDistanceAtStart;
	
	protected override void	Awake()
	{
		base.Awake();
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
		float distanceSum = _storms.Aggregate(0f, (sum, storm) => sum + Vector3.Distance(storm.transform.position, transform.position));
		float average = distanceSum / _storms.Length;

		return (average);
	}
	
	public void	ComeCloser()
	{
		foreach (Storm storm in _storms)
			storm.ComeCloser();
	}
}
