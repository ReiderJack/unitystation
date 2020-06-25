using System;
using CustomVariables;
using CustomAttributes;
using Messages.Server.Audio;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Audio.Containers
{
	[CreateAssetMenu(fileName = "AudioSourceData", menuName = "ScriptableObjects/Audio/AudioSourceData", order = 0)]
	public class AudioSourceData : AudioEvent
	{
		[SerializeField] private AudioClip audioClip;
		[MinMaxFloatRange(0,1)] public RangedMinMaxFloat volume;
		[MinMaxFloatRange(0.25f,3)] public RangedMinMaxFloat pitch;
		[SerializeField] private float maxDistance;

		/// <summary>
		/// Sets variables of this data and plays audio source
		/// </summary>
		/// <param name="audioSource"></param>
		public override void Play(AudioSource audioSource)
		{
			SetVariables(audioSource);

			audioSource.Play();
		}

		/// <summary>
		/// Sets data for audio source
		/// </summary>
		/// <param name="audioSource"></param>
		private void SetVariables(AudioSource audioSource)
		{
			audioSource.clip = audioClip;
			audioSource.volume = volume.GetRandom();
			audioSource.pitch = pitch.GetRandom();
			audioSource.maxDistance = maxDistance;
		}

		/// <summary>
		/// Plays sound for clients in range
		/// It will send message to play sound only to clients in range
		/// Client which was not in range when audio started playing won't hear it
		/// </summary>
		/// <param name="audioSource"> GameObject with Audio Source </param>
		public void PlayNetworkInRange(GameObject audioSource)
		{
			int? clipNumber = AudioManager.Instance.AudioClips.GetClipNumberInArray(audioClip);
			if (clipNumber == null) return;

			PlayAudioMessage.PlaySound(clipNumber.Value, volume.GetRandom(), pitch.GetRandom(), maxDistance, audioSource);
		}

		/// <summary>
		/// Plays sound for clients in range
		/// It will send message to play sound only to clients in range
		/// Client which was not in range when audio started playing won't hear it
		/// </summary>
		/// <param name="audioSource"> GameObject with Audio Source </param>
		/// <param name="maximumDistance"> Max distance </param>
		public void PlayNetworkInRange(GameObject audioSource, float maximumDistance)
		{
			int? clipNumber = AudioManager.Instance.AudioClips.GetClipNumberInArray(audioClip);
			if (clipNumber == null) return;

			PlayAudioMessage.PlaySound(clipNumber.Value, volume.GetRandom(), pitch.GetRandom(), maximumDistance, audioSource);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="isGlobal"> To play for all players </param>
		/// <returns> Audio source which currently plays the audio </returns>
		public AudioSource PlayNetworkAtPosition(Vector3 worldPosition, bool isGlobal = false)
		{
			return null;
		}

		/// <summary>
		/// Attach audio to game object until it's playing
		/// </summary>
		/// <param name="transform"> Transform of the object to attach audio to </param>
		/// <param name="isGlobal"> To play for all players </param>
		/// <returns> Audio source which currently plays the audio </returns>
		public AudioSource PlayNetworkOnObject(Transform transform, bool isGlobal = false)
		{
			return null;
		}

		private void OnValidate()
		{
			if (maxDistance < 0)
			{
				Debug.Log($"<color=red>Error: </color>Maximum distance must be positive in {this.name}!");
				maxDistance = 0;
			}
		}
	}
}