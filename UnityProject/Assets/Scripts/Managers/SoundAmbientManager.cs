using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Audio.Containers;
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

		/// <summary>
		/// Cache of audioSources on the Manager
		/// </summary>
		private List<AudioSource> ambientTracks = new List<AudioSource>();
		public List<AudioSource> AmbientTracks => ambientTracks;

		public AudioSource CurrentAmbientTrack { get; set; }

		[SerializeField] private AudioClipsArray audioClips = null;
		[SerializeField] private AudioMixerGroup audioMixerGroup = null;

		private void Awake()
		{
			SetVolumeWithPlayerPrefs();
		}

		private void SetVolumeWithPlayerPrefs()
		{
			if (PlayerPrefs.HasKey(PlayerPrefKeys.AmbientVolumeKey))
			{
				SetVolumeForAllAudioSources(PlayerPrefs.GetFloat(PlayerPrefKeys.AmbientVolumeKey));
			}
			else
			{
				SetVolumeForAllAudioSources(1f);
			}
		}

		/// <summary>
		/// Stops every track and starts playing new one
		/// </summary>
		/// <param name="ambientTrackName"> Name of a new track to play </param>
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

		/// <summary>
		/// Plays track and sets volume if player has prefs
		/// </summary>
		/// <param name="track"> Track to play </param>
		private static void PlayAmbientTrack(AudioSource track)
		{
			Logger.Log($"Playing ambient track: {track.name}", Category.SoundFX);
			Instance.CurrentAmbientTrack = track;

			if (PlayerPrefs.HasKey("AmbientVol"))
			{
				track.volume = Mathf.Clamp(PlayerPrefs.GetFloat("AmbientVol"), 0f, 0.25f);
			}

			track.Play();
		}

		/// <summary>
		/// Stops every clip in the ambientTracks list
		/// </summary>
		public static void StopAllAudioSources()
		{
			foreach (AudioSource source in Instance.ambientTracks)
			{
				source.Stop();
			}
		}

		/// <summary>
		/// Sets all ambient tracks to a certain volume
		/// </summary>
		/// <param name="newVolume"></param>
		public static void SetVolumeForAllAudioSources(float newVolume)
		{
			float volume = Mathf.Clamp(newVolume, 0f, 0.25f);
			foreach (AudioSource s in Instance.ambientTracks)
			{
				s.volume = volume;
			}

			PlayerPrefs.SetFloat(PlayerPrefKeys.AmbientVolumeKey, volume);
			PlayerPrefs.Save();
		}

		#region Editor

		private void OnValidate()
		{
			// Delay to safely initialize objects and avoid warnings
			UnityEditor.EditorApplication.delayCall += () => HandleAudioSources();
		}

		/// <summary>
		/// Every time audioClips is changed in the editor
		/// Manager creates GameObjects with AudiosSource for each clip
		/// </summary>
		private void HandleAudioSources()
		{
			if (audioClips == null)
			{
				var audioSources = gameObject.GetComponentsInChildren<AudioSource>(true);
				DestroyGameObjects(audioSources);
				ambientTracks.Clear();
				return;
			}

			// Compare AudioSources on the manager and create new
			var managerAudioSources = gameObject.GetComponentsInChildren<AudioSource>(true);
			CreateNewAudioSources(GetMissingClips(managerAudioSources));

			// Clearing Manager children from audio sources which are not in audioClips
			DestroyGameObjects(GetRedundantAudioSources(managerAudioSources));

			// Cache AudioSources on the manager for runtime use
			managerAudioSources = gameObject.GetComponentsInChildren<AudioSource>(true);
			CacheAudioSources(managerAudioSources);
		}

		/// <summary>
		/// Destroys GameObjects with AudioSource component
		/// </summary>
		/// <param name="audioSources"> GameObjects to destroy </param>
		private void DestroyGameObjects(IEnumerable<AudioSource> audioSources)
		{
			foreach (var source in audioSources)
			{
				DestroyImmediate(source.gameObject);
			}
		}

		/// <summary>
		/// Creates GameObjects with AudioSource component for each clip
		/// </summary>
		/// <param name="newClips"> Clips to add as AudioSources </param>
		private void CreateNewAudioSources(IEnumerable<AudioClip> newClips)
		{
			foreach (var clip in newClips)
			{
				var newGameObject = new GameObject(clip.name);
				newGameObject.transform.parent = gameObject.transform;

				var audioSource = newGameObject.AddComponent<AudioSource>();
				audioSource.outputAudioMixerGroup = audioMixerGroup;
				audioSource.clip = clip;
				audioSource.playOnAwake = false;
			}
		}

		/// <summary>
		/// Get clips which are not on the manager as AudioSources
		/// </summary>
		/// <returns> Array of new clips </returns>
		private IEnumerable<AudioClip> GetMissingClips(IEnumerable<AudioSource> managerAudioSources)
		{
			var newClips = new List<AudioClip>();

			foreach (var clip in audioClips.AudioClips)
			{
				if(managerAudioSources.Any( a => a.name == clip.name)) continue;

				newClips.Add(clip);
			}

			return newClips;
		}

		/// <summary>
		/// Compares clips names with Manager children which have AudioSource component
		/// </summary>
		/// <param name="audioSources"></param>
		/// <returns> AudiosSources on Manager which are not in audioClips</returns>
		private IEnumerable<AudioSource> GetRedundantAudioSources(IEnumerable<AudioSource> audioSources)
		{
			var redundantAudioSources = new List<AudioSource>();

			foreach (var source in audioSources)
			{
				if(audioClips.AudioClips.Any(c => c.name == source.name)) continue;
				redundantAudioSources.Add(source);
			}

			return redundantAudioSources;
		}

		/// <summary>
		/// Cache audioSources so we can control them at runtime
		/// </summary>
		/// <param name="managerAudioSources"> AudioSources on the manager </param>
		private void CacheAudioSources(IEnumerable<AudioSource> managerAudioSources)
		{
			foreach (var audioSource in managerAudioSources)
			{
				if (ambientTracks.Contains(audioSource) == false
					&& audioClips.AudioClips.Any(a => a.name == audioSource.name))
				{
					ambientTracks.Add(audioSource);
				}
			}
		}

		#endregion
	}
}