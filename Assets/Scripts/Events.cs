using UnityEngine;
using UnityEngine.VFX;

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

	public static void	spawnFallingBoxShockEffect(GameObject gameObject)
	{
		if (_instance == null)
		{
			Debug.LogError("There is no instance of the Events class but an eventt function has been called !");
			return;
		}
		Animator	effect = Instantiate(_instance._smokeEffect , gameObject.transform.position,
				gameObject.transform.rotation);
		float effectDuration = effect.GetCurrentAnimatorStateInfo(0).length;
		Destroy(effect.gameObject, effectDuration);
		if (AudioManager.TryAndGetInstance(out AudioManager audioManager))
			audioManager.playAudioEffect("BoxFall", gameObject.transform.position, 1);
	}
	
	public static void	spawnPollosShockEffect(GameObject gameObject)
	{
		if (_instance == null)
		{
			Debug.LogError("There is no instance of the Events class but an event function has been called !");
			return;
		}
		{
			Animator	effect = Instantiate(_instance._smokeEffect , gameObject.transform.position,
					gameObject.transform.rotation);
			float effectDuration = effect.GetCurrentAnimatorStateInfo(0).length;

			Destroy(effect.gameObject, effectDuration);
		}
		{
			VisualEffect	vfx = Instantiate(_instance._pollosShockVFX, gameObject.transform.position,
					gameObject.transform.rotation);
			float	effectDuration = 2.0f;
			
			Destroy(vfx.gameObject, effectDuration);
		}
	}
}
