using UnityEngine;

public class Events : MonoBehaviour
{
	private static Events	_instance;

	[SerializeField]
	private Animator		_smokeEffect;
	
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
		AudioBank	audioBank = AudioBank.GetInstance();
		if (audioBank == null)
			Debug.LogError("No instance of the AudioBank class !");
		else
			audioBank.PlayBoxFallAudio(gameObject.transform.position, 1.0f);
	}
	
	public static void	spawnPollosShockEffect(GameObject gameObject)
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
		AudioBank	audioBank = AudioBank.GetInstance();
		if (audioBank == null)
			Debug.LogError("No instance of the AudioBank class !");
		else
			audioBank.PlayChickenAudio(gameObject.transform.position, 1.0f);
	}
}
