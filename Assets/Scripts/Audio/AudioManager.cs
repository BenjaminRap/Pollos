using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioManager	_instance;

	[SerializeField]
	private AudioSource			_audioSourcePrefab;

	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the AudioManager class !");
			Destroy(this);
			return ;
		}
		_instance = this;
	}
	
	public static AudioManager	GetInstance()
	{
		return (_instance);
	}
	
	public void	playAudioEffect(AudioClip audioClip, Vector3 audioSourcePoint, float volume)
	{
		AudioSource	audioSource = Instantiate(_audioSourcePrefab, audioSourcePoint, Quaternion.identity);
		
		audioSource.clip = audioClip;
		audioSource.volume = volume;

		audioSource.Play();

		Destroy(audioSource, audioClip.length);
	}
}
