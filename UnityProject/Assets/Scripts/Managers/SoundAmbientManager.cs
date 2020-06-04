using UnityEngine;
using System;
using System.Collections.Generic;
using Audio;

namespace Audio.Managers
{
	public class SoundAmbientManager : MonoBehaviour
	{
		private static SoundAmbientManager soundAmbientManager;
		public static SoundAmbientManager Instance
		{
			get
			{
				if (soundAmbientManager == null)
				{
					soundAmbientManager = FindObjectOfType<SoundAmbientManager>();
				}

				return soundAmbientManager;
			}
		}

		public AudioSource ambientTrack = null;

		public List<AudioSource> ambientTracks = new List<AudioSource>();
		[SerializeField] private AudioClipsArray audioClips = null;

		/// <summary>
		/// Stops every clip in the ambientTracks list
		/// </summary>
		public static void StopAmbient()
		{
			foreach (AudioSource source in Instance.ambientTracks)
			{
				source.Stop();
			}
		}

		private void Awake()
		{
			Init();
		}

		private void Init()
		{
			if (PlayerPrefs.HasKey(PlayerPrefKeys.AmbientVolumeKey))
			{
				AmbientVolume(PlayerPrefs.GetFloat(PlayerPrefKeys.AmbientVolumeKey));
			}
			else
			{
				AmbientVolume(1f);
			}

			var audioSources = gameObject.GetComponentsInChildren<AudioSource>(true);

			foreach (var audioSource in audioSources)
			{
				if (audioSource.gameObject.CompareTag("AmbientSound"))
				{
					ambientTracks.Add(audioSource);
				}
			}
		}

		public static void PlayAmbience(string ambientTrackName)
		{
			foreach (var track in Instance.ambientTracks)
			{
				if (track.name == ambientTrackName)
				{
					PlayAmbientTrack(track);
				}
				else
				{
					track.Stop();
				}
			}
		}

		private static void PlayAmbientTrack(AudioSource track)
		{
			Logger.Log($"Playing ambient track: {track.name}", Category.SoundFX);
			Instance.ambientTrack = track;

			if (PlayerPrefs.HasKey("AmbientVol"))
			{
				track.volume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVol"), 0f, 0.25f);
			}

			track.Play();
		}

		/// <summary>
		/// Sets all ambient tracks to a certain volume
		/// </summary>
		/// <param name="volume"></param>
		public static void AmbientVolume(float volume)
		{
			volume = Mathf.Clamp(volume, 0f, 0.25f);
			foreach (AudioSource s in Instance.ambientTracks)
			{
				s.volume = volume;
			}

			PlayerPrefs.SetFloat(PlayerPrefKeys.AmbientVolumeKey, volume);
			PlayerPrefs.Save();
		}
	}
}