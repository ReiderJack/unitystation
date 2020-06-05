using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine.Audio;

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
		[SerializeField] private AudioMixerGroup audioMixerGroup = null;

		private void Awake()
		{
			SetVolume();
		}

		private void SetVolume()
		{
			if (PlayerPrefs.HasKey(PlayerPrefKeys.AmbientVolumeKey))
			{
				AmbientVolume(PlayerPrefs.GetFloat(PlayerPrefKeys.AmbientVolumeKey));
			}
			else
			{
				AmbientVolume(1f);
			}
		}

		/// <summary>
		/// Every time clips are added to the list
		/// Manager creates a new game object with Audio component
		/// which has new clip in it
		/// </summary>
		private void OnValidate()
		{
			if (audioClips == null)
			{
				gameObject.transform.DetachChildren();
				ambientTracks.Clear();
				return;
			}

			var managerAudioSources = DetachRedundantAudioSources(gameObject.GetComponentsInChildren<AudioSource>(true));
			CreateNewAudioSources(GetMissingClips(managerAudioSources));
			CacheAudioSources(managerAudioSources);
		}

		private AudioSource[] DetachRedundantAudioSources(AudioSource[] audioSources)
		{
			var audioSourcesList = audioSources.ToList();
			Debug.Log(audioSources.Length.ToString());
			for (int i = audioSources.Length - 1; i >= 0; i--)
			{
				if(audioClips.AudioClips.Any(a => a.name == audioSources[i].name)) continue;
				audioSources[i].transform.parent = null;
				audioSourcesList.RemoveAt(i);
			}

			return audioSourcesList.ToArray();
		}

		/// <summary>
		/// Cache audioSources so we can control them at runtime
		/// </summary>
		/// <param name="managerAudioSources"> AudioSources on the manager</param>
		private void CacheAudioSources(IEnumerable<AudioSource> managerAudioSources)
		{
			foreach (var audioSource in managerAudioSources)
			{
				if (ambientTracks.Contains(audioSource) == false)
				{
					ambientTracks.Add(audioSource);
				}
			}
		}

		/// <summary>
		/// Creates GameObjects with AudioSource component
		/// based on audio clips in the array
		/// </summary>
		/// <param name="newClips"> Clips to add as AudioSources</param>
		private void CreateNewAudioSources(IEnumerable<AudioClip> newClips)
		{
			foreach (var clip in newClips)
			{
				var newGameObject = new GameObject(clip.name);
				newGameObject.AddComponent<AudioSource>();

				var audioSource = newGameObject.GetComponent<AudioSource>();

				audioSource.outputAudioMixerGroup = audioMixerGroup;
				audioSource.clip = clip;
				audioSource.playOnAwake = false;

				newGameObject.transform.parent = gameObject.transform;
			}
		}

		/// <summary>
		/// Gets clips which are not
		/// yet added to Manager as AudioSources
		/// </summary>
		/// <returns> Array of new clips </returns>
		private IEnumerable<AudioClip> GetMissingClips(IEnumerable<AudioSource> managerAudioSources)
		{
			var newClips = new List<AudioClip>();

			foreach (var clip in audioClips.AudioClips)
			{
				if(IsClipOnManager(clip, managerAudioSources)) continue;

				newClips.Add(clip);
			}

			return newClips;
		}

		/// <summary>
		/// Checks if there is a prefab on Manager with clip name
		/// </summary>
		/// <param name="clipToCheck"> Clip to Check </param>
		/// <returns> True is clip is in any AudioSource on the manager</returns>
		private bool IsClipOnManager(AudioClip clipToCheck, IEnumerable<AudioSource> managerAudioSources)
		{
			foreach (var audioSource in managerAudioSources)
			{
				if (audioSource.name == clipToCheck.name)
				{
					return true;
				}
			}

			return false;
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
		/// Stops every clip in the ambientTracks list
		/// </summary>
		public static void StopAmbient()
		{
			foreach (AudioSource source in Instance.ambientTracks)
			{
				source.Stop();
			}
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