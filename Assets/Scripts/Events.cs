using System;
using UnityEngine;

[Serializable]
public class Effect {
	[SerializeField] private GameObject	_prefab;
	[SerializeField] private float	_duration;
	
	public GameObject	Prefab { get => _prefab; }
	public float	Duration { get => _duration; }
};

public class Events : MonoBehaviourSingleton<Events>
{
	[SerializeField] private Effect	_smokeEffect;
	[SerializeField] private Effect	_pollosShockVFX;
	
	private static void	InstantiateTemporaryEffect(Effect effect, Transform transform)
	{
		if (!Level.TryGetInstance(out Level level))
			return ;
		GameObject	instance = Instantiate(effect.Prefab, transform.position, transform.rotation, level.GetRotatableChild());
		Destroy(instance, effect.Duration);
	}

	public static void	SpawnFallingBoxShockEffect(Transform transform)
	{
		if (!TryGetInstance(out Events events))
			return ;

		InstantiateTemporaryEffect(events._smokeEffect, transform);
		if (AudioManager.TryGetInstance(out AudioManager audioManager))
			audioManager.PlayAudioEffect("BoxFall", transform.position, 1);
	}
	
	public static void	SpawnPollosShockEffect(Transform transform)
	{
		if (!TryGetInstance(out Events events))
			return ;
		InstantiateTemporaryEffect(events._smokeEffect, transform);
		InstantiateTemporaryEffect(events._pollosShockVFX, transform);
	}
}
