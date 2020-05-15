using System;
using System.Collections.Generic;
using SO.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SoundManagers
{
	public class SoundEffectsManager : MonoBehaviour
	{
		private static SoundEffectsManager soundEffectsManager;

		public static SoundEffectsManager Instance
		{
			get
			{
				if (soundEffectsManager == null)
				{
					soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
				}

				return soundEffectsManager;
			}
		}

		public AudioMixerGroup soundEffectsMixer = null;

		[Range(0f,1f)]
		public static float Volume;

		[SerializeField] private AudioClipsList clipsList;

		private void OnEnable()
		{
			if (PlayerPrefs.HasKey(PlayerPrefKeys.SoundEffectsVolumeKey))
			{
				OnSoundEffectsVolumeChange(PlayerPrefs.GetFloat(PlayerPrefKeys.MasterVolumeKey));
			}
			else
			{
				OnSoundEffectsVolumeChange(1f);
			}
		}

		private void OnDisable()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sound Effects Volume
		/// </summary>
		/// <param name="volume"></param>
		public static void OnSoundEffectsVolumeChange(float volume)
		{
			Volume = volume;
			PlayerPrefs.SetFloat(PlayerPrefKeys.SoundEffectsVolumeKey, volume);
			PlayerPrefs.Save();
		}
	}
}