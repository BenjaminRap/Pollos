using System;
using UnityEngine;

[Serializable]
public class Effect {
	[SerializeField] private GameObject	_prefab;
	[SerializeField] private float	_duration;
	
	public GameObject	Prefab { get => _prefab; }
	public float	Duration { get => _duration; }
};

public class Events : MonoBehaviour
{
	private static Events	_instance;

	[SerializeField] private Effect	_smokeEffect;
	[SerializeField] private Effect	_pollosShockVFX;
	
	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the Events class !");
			Destroy(gameObject);
			return ;
		}
		_instance = this;
	}
	
	private static void	InstantiateTemporaryEffect(Effect effect, Transform transform)
	{
		if (!Level.TryGetInstance(out Level level))
			return ;
		GameObject	instance = Instantiate(effect.Prefab, transform.position, transform.rotation, level.GetRotatableChild());
		Destroy(instance, effect.Duration);
	}

	public static void	SpawnFallingBoxShockEffect(Transform transform)
	{
		if (_instance == null)
		{
			Debug.LogError("There is no instance of the Events class but an eventt function has been called !");
			return;
		}

		InstantiateTemporaryEffect(_instance._smokeEffect, transform);
		if (AudioManager.TryGetInstance(out AudioManager audioManager))
			audioManager.PlayAudioEffect("BoxFall", transform.position, 1);
	}
	
	public static void	SpawnPollosShockEffect(Transform transform)
	{
		if (_instance == null)
		{
			Debug.LogError("There is no instance of the Events class but an event function has been called !");
			return;
		}
		InstantiateTemporaryEffect(_instance._smokeEffect, transform);
		InstantiateTemporaryEffect(_instance._pollosShockVFX, transform);
	}
}
