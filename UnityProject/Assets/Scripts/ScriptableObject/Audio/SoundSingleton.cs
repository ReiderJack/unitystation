using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

namespace SO.Audio
{
	[CreateAssetMenu(fileName = "SoundSingleton", menuName = "Singleton/SoundSingleton")]
	public class SoundSingleton : SingletonScriptableObject<SoundSingleton>
	{
		public List<AudioClip> audioClips = new List<AudioClip>();

		private static bool Initialised = false;

		void OnEnable()
		{
			Setup();
		}

		private void Setup()
		{
			// Sort all AudioClips by name
			audioClips = audioClips.OrderBy(s => s.name).ToList();

		}
		private void OnValidate()
		{
			// Find all AudioClips in the project and add them to the list.
			var audioClipList = Resources.LoadAll<AudioClip>("Sounds").OrderBy(s => s.name).ToList();

			foreach (var audioClip in audioClipList)
			{
				if (audioClips.Contains(audioClip)) continue;
				audioClips.Add(audioClip);
			}
		}
	}
}