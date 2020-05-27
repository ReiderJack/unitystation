using System;
using UnityEngine;
using CustomVariables.Float;
using SO.Audio;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace SO.Audio
{
	[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/Sounds/SoundList")]
	public class SoundListEvent : ScriptableObject
	{
		[SerializeField]
		private AudioClip[] audioClips;

		[SerializeField]
		private FloatRanged volume = new FloatRanged();

		[SerializeField]
		[FloatRange(0, 2)]
		private FloatRanged pitch = new FloatRanged();

		public void Play(AudioSource audioSource)
		{
			if (audioClips.Length == 0) return;

			audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
			var clip = audioSource.clip;
			audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
			audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
			audioSource.Play();
		}

		public void PlayAtPosition(GameObject recipient, Vector3 position)
		{

			var rndClip = audioClips[Random.Range(0, audioClips.Length)];
			int indexClip = SoundSingleton.Instance.audioClips.IndexOf(rndClip);

			var rndPitch = Random.Range(pitch.minValue, pitch.maxValue);
			var rndVolume = Random.Range(volume.minValue, volume.maxValue);

			PlaySoundMessageV2.Send(recipient, position, indexClip, rndPitch, rndVolume);
		}
	}
}