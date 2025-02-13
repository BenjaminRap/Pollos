using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>A temporary class that is used to serialized a dictionnary in the inspector.</summary>
[Serializable]
public class	AudioEffectSerializable
{
	[SerializeField]
	public string		audioName;
	[SerializeField]
	public AudioClip[]	clips;
}

/// <summary>This class is a singleton that manages the temporary sound of a scene.
/// It can add sounds, associated with an audioName, and destroy the gameObject
/// after the audio played.</summary>
public class	AudioManager : MonoBehaviour
{
	private static AudioManager				_instance;

	/// <summary>The prefab that will be instanciated when playing a sound.</summary>
	[SerializeField]
	private AudioSource						_audioSourcePrefab;
	/// <summary>The temporary list that will be converted into a dictionnary.</summary>
	[SerializeField]
	private AudioEffectSerializable[]		_audioEffectsSerializable;
	
	/// <summary>A dictionnary of audioName (key) and list of AudioClip (value).</summary>
	private Dictionary<string, AudioClip[]>	_audioEffects = new();

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
	
	/// <summary>Get the instance of this singleton if there is one.</summary>
	/// <param name="audioManager">This variable will be set to the instance value.</param>
	/// <returns>True if there is an instance, false otherwise.</returns>
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
	
	/// <summary>Returns a random audioclip that corresponds to the audioName.
	/// If audioName is invalid or if the list associated to it is empty, print an
	/// error message and return null.</summary>
	/// <param name="audioName">The name of the audio, the key of the dictionnary.</param>
	/// <returns>An Random AudioClip or null</returns>
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
		catch (System.Exception)
		{
			return (null);
		}
	}
	
	/// <summary>Player a random effect associated to audioName at the position audioSourcePoint
	/// with the volume in parameter.</summary>
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
