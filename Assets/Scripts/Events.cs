using UnityEngine;
using UnityEngine.VFX;

/// <summary>This class is a singleton that stores all the static event functions
/// that can be assigned to a prefab</summary>
public class Events : MonoBehaviour
{
	private static Events	_instance;

	[SerializeField]
	private Animator		_smokeEffect;
	[SerializeField]
	private VisualEffect	_pollosShockVFX;
	
	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the Events class !");
			Destroy(this);
			return ;
		}
		_instance = this;
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

		if (!Level.TryAndGetInstance(out Level level))
			return ;
		Animator	effect = Instantiate(_instance._smokeEffect , transform.position,
				transform.rotation, level.GetRotableChild());
		float effectDuration = effect.GetCurrentAnimatorStateInfo(0).length;
		Destroy(effect.gameObject, effectDuration);
		if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
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
		if (!Level.TryAndGetInstance(out Level level))
			return ;
		{
			Animator	effect = Instantiate(_instance._smokeEffect , transform.position,
					transform.rotation, level.GetRotableChild());
			float effectDuration = effect.GetCurrentAnimatorStateInfo(0).length;

			Destroy(effect.gameObject, effectDuration);
		}
		{
			VisualEffect	vfx = Instantiate(_instance._pollosShockVFX, transform.position,
					transform.rotation, level.GetRotableChild());
			float	effectDuration = 2.0f;
			
			Destroy(vfx.gameObject, effectDuration);
		}
	}
}
