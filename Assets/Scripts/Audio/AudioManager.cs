using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class	AudioEffectSerializable
{
	[SerializeField]
	public string		audioName;
	[SerializeField]
	public AudioClip	clip;
}

public class	AudioManager : MonoBehaviour
{
	private static AudioManager				_instance;

	[SerializeField]
	private AudioSource						_audioSourcePrefab;
	[SerializeField]
	private AudioEffectSerializable[]		_audioEffectsSerializable;
	
	private Dictionary<string, AudioClip>	_audioEffects = new Dictionary<string, AudioClip>();

	private void	Start()
	{
		foreach (AudioEffectSerializable audioEffect in _audioEffectsSerializable)
		{
			_audioEffects.Add(audioEffect.audioName, audioEffect.clip);
		}
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
	
	public void	playAudioEffect(string audioName, Vector3 audioSourcePoint, float volume)
	{
		AudioClip	audioClip = _audioEffects[audioName];

		if (audioClip == null)
		{
			Debug.LogError("Unknown audio clip !");
			return ;
		}
		AudioSource	audioSource = Instantiate(_audioSourcePrefab, audioSourcePoint, Quaternion.identity);
		
		audioSource.clip = audioClip;
		audioSource.volume = volume;

		audioSource.Play();

		Destroy(audioSource.gameObject, audioClip.length);
	}
}
