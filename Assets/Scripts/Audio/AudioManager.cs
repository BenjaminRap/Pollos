using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class	AudioEffectSerializable
{
	[SerializeField] public string	audioName;
	[SerializeField] public AudioClip[]	clips;
}

public class	AudioManager : MonoBehaviour
{
	private static AudioManager	_instance;

	[SerializeField] private AudioSource	_audioSourcePrefab;
	[SerializeField] private AudioEffectSerializable[]	_audioEffectsSerializable;
	
	private readonly Dictionary<string, AudioClip[]>	_audioEffects = new();

	private void	Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instances of the AudioManager class !");
			Destroy(gameObject);
			return ;
		}
		foreach (AudioEffectSerializable audioEffect in _audioEffectsSerializable)
		{
			_audioEffects.Add(audioEffect.audioName, audioEffect.clips);
		}
		_instance = this;
	}
	
	public static bool	TryGetInstance(out AudioManager audioManager)
	{
		audioManager = _instance;
		if (_instance == null)
		{
			Debug.LogError("The AudioManager class has no instance !");
			return (false);
		}
		return (true);
	}
	
	private AudioClip	PickRandomAudio(string audioName)
	{
		try
		{
			AudioClip[]	audioClips = _audioEffects[audioName];
			int audioCount = audioClips.Count();
			if (audioCount == 0)
			{
				Debug.LogError("The sound : " + audioName + " has no clip attached");
				return (null);
			}
			int randomIndex = UnityEngine.Random.Range(0, audioCount - 1);
			return (audioClips[randomIndex]);
		}
		catch (Exception)
		{
			return (null);
		}
	}
	
	public void	PlayAudioEffect(string audioName, Vector3 audioSourcePoint, float volume)
	{
		AudioClip	audioClip = PickRandomAudio(audioName);

		if (audioClip == null)
			return ;
		if (!Level.TryGetInstance(out Level level))
			return ;
		AudioSource	audioSource = Instantiate(_audioSourcePrefab, audioSourcePoint, Quaternion.identity, level.GetRotatableChild());
		
		audioSource.clip = audioClip;
		audioSource.volume = volume;

		audioSource.Play();

		Destroy(audioSource.gameObject, audioClip.length);
	}
}
