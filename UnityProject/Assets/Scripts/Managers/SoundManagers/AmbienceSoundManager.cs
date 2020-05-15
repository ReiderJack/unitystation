using System;
using SO.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SoundManagers
{
	public class AmbienceSoundManager : MonoBehaviour
	{
		private static AmbienceSoundManager AmbienceSound;

		public static AmbienceSoundManager Instance
		{
			get
			{
				if (AmbienceSound == null)
				{
					AmbienceSound = FindObjectOfType<AmbienceSoundManager>();
				}

				return AmbienceSound;
			}
		}

		public AudioSource ambienceSource;

		[Range(0f,1f)]
		public static float Volume = 1;

		[SerializeField]
		private  AudioClipsList musicClips = null;

		private void OnEnable()
		{
			//Ambient Volume Preference
			if (PlayerPrefs.HasKey(PlayerPrefKeys.AmbientVolumeKey))
			{
				AmbientVolume(PlayerPrefs.GetFloat(PlayerPrefKeys.AmbientVolumeKey));
			}
			else
			{
				AmbientVolume(1f);
			}
		}

		public static void StopAmbient()
		{

		}

		public static void PlayAmbience(string ambientTrackName)
		{

		}

		/// <summary>
		/// Sets all ambient tracks to a certain volume
		/// </summary>
		/// <param name="volume"></param>
		public static void AmbientVolume(float volume)
		{
			volume = Mathf.Clamp(volume, 0f, 0.25f);
			Volume = volume;
			PlayerPrefs.SetFloat(PlayerPrefKeys.AmbientVolumeKey, volume);
			PlayerPrefs.Save();
		}
	}
}