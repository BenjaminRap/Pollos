using System;
using UnityEngine;

/// <summary>This class stores all the variables necessary for an Effect :
/// its prefab and duration.</summary>
[Serializable]
public class Effect {
	[SerializeField]
	private GameObject	_prefab;
	[SerializeField]
	private float		_duration;
	
	public GameObject	Prefab { get => _prefab; }
	public float		Duration { get => _duration; }
};

/// <summary>This class is a singleton that stores all the static event functions
/// that can be assigned to a prefab</summary>
public class Events : MonoBehaviour
{
	private static Events	_instance;

	[SerializeField]
	private Effect			_smokeEffect;
	[SerializeField]
	private Effect			_pollosShockVFX;
	
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
	
	/// <summary>Instantiate the Effect.Prefab and destroy it after Effect.Duration seconds.
	/// The parent of the instanciated effect will be the current level rotable</summary>
	/// <param name="transform">The effect will be instanciated with the transform.position and rotation.</param>
	private static void	InstantiateTemporaryEffect(Effect effect, Transform transform)
	{
		if (!Level.TryGetInstance(out Level level))
			return ;
		GameObject	instance = Instantiate(effect.Prefab, transform.position, transform.rotation, level.GetRotatableChild());
		Destroy(instance, effect.Duration);
	}

	/// <summary>Spawn a smoke effect and play a falling box sound.</summary>
	/// <param name="gameObject">The box gameObject.</param>
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
	
	/// <summary>Spawn a smoke effect, a feather vfx and a pollos hurt sound.</summary>
	/// <param name="gameObject">The pollos gameObject.</param>
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
