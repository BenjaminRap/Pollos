using UnityEngine;

public class AudioBank : MonoBehaviour
{
	private static AudioBank	_instance;
	
	[SerializeField]
	private AudioClip			_winAudio;
	[SerializeField]
	private AudioClip			_defeatAudio;

	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the class AudioBank !");
			Destroy(this);
			return ;
		}
		_instance = this;
	}
	
	public static AudioBank	GetInstance()
	{
		return (_instance);
	}
	
	private bool	getAudioManager(out AudioManager audioManager)
	{
		audioManager = AudioManager.GetInstance();
		
		if (audioManager == null)
		{
			Debug.LogError("The AudioManager class has no instance but a Play..Audio has been called !");
			return (false);
		}	
		return (true);
	}
	
	public void	PlayWinAudio(Vector3 audioSourcePoint, float volume)
	{
		if (!getAudioManager(out AudioManager audioManager))
			return ;
		audioManager.playAudioEffect(_winAudio, audioSourcePoint, volume);
	}
	
		public void	PlayDefeatAudio(Vector3 audioSourcePoint, float volume)
	{
		if (!getAudioManager(out AudioManager audioManager))
			return ;
		audioManager.playAudioEffect(_defeatAudio, audioSourcePoint, volume);
	}
}
