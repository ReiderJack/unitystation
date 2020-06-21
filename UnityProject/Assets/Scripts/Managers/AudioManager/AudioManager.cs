using System.Runtime.Remoting.Messaging;
using Audio.Containers;
using Mirror;
using UnityEngine;

namespace Audio
{
	public class AudioManager : MonoBehaviour
	{
		private static AudioManager audioManager;

		public static AudioManager Instance
		{
			get
			{
				if (!audioManager)
				{
					audioManager = FindObjectOfType<AudioManager>();
				}

				return audioManager;
			}
		}

		private AudioClipsArray audioClips;

		public AudioClipsArray AudioClips => audioClips;

		public static void Play(int clipNumber, float volume, float pitch, uint netId)
		{
			if (netId == NetId.Empty) return;
			if (!NetworkIdentity.spawned.ContainsKey(netId)) return;

			var audioSource = NetworkIdentity.spawned[netId].transform.gameObject.GetComponent<AudioSource>();

			if (audioSource == null) return;
			audioSource.clip = AudioManager.Instance.audioClips.AudioClips[clipNumber];
			audioSource.volume = volume;
			audioSource.pitch = pitch;

			audioSource.Play();
		}
	}
}