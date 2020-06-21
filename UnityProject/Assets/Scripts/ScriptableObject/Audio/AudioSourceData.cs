﻿using Messages.Server.Audio;
using UnityEngine;

namespace Audio.Containers
{
	[CreateAssetMenu(fileName = "AudioSourceData", menuName = "ScriptableObjects/Audio/AudioSourceData", order = 0)]
	public class AudioSourceData : AudioEvent
	{
		[SerializeField] private AudioClip audioClip;
		[SerializeField] private float volume;
		[SerializeField] private float pitch;

		public override void Play(AudioSource audioSource)
		{
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.pitch = pitch;

			audioSource.Play();
		}

		public override void PlayServer(GameObject audioSource)
		{
			int? id = AudioManager.Instance.AudioClips.GetClipNumberInArray(audioClip);
			if (id == null) return;

			PlayAudioMessage.PlaySound(id.Value, volume, pitch, audioSource);
		}
	}
}