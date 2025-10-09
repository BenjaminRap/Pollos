using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AudioEffectSerializable {
	[SerializeField] public string audioName;
	[SerializeField] public AudioClip[] clips;
}

public class AudioManager : MonoBehaviourSingleton<AudioManager> {
	[SerializeField] private AudioSource _audioSourcePrefab;
	[SerializeField] private AudioEffectSerializable[] _audioEffectsSerializable;

	private readonly Dictionary<string, AudioClip[]> _audioEffects = new();

	protected override void Awake() {
		base.Awake();
		foreach (AudioEffectSerializable audioEffect in _audioEffectsSerializable) {
			_audioEffects.Add(audioEffect.audioName, audioEffect.clips);
		}
	}

	private AudioClip PickRandomAudio(string audioName) {
		if (!_audioEffects.TryGetValue(audioName, out AudioClip[] audioClips)
			|| audioClips.Count() == 0) {
			Debug.LogError("The sound : " + audioName + " has no clip attached");
			return null;
		}
		int randomIndex = UnityEngine.Random.Range(0, audioClips.Count() - 1);

		return audioClips[randomIndex];
	}

	public void PlayAudioEffect(string audioName, Vector3 audioSourcePoint, float volume) {
		AudioClip audioClip = PickRandomAudio(audioName);

		if (audioClip == null)
			return;
		if (!Cube.TryGetInstance(out Cube cube))
			return;
		AudioSource audioSource = Instantiate(_audioSourcePrefab, audioSourcePoint, Quaternion.identity, cube.transform);

		audioSource.clip = audioClip;
		audioSource.volume = volume;

		audioSource.Play();

		Destroy(audioSource.gameObject, audioClip.length);
	}
}
