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
		[MinMaxFloatRange(0,3)] public RangedMinMaxFloat pitch;
		[SerializeField] private float maxDistance;
		[SerializeField] private AnimationCurve animationCurve;

		public override void Play(AudioSource audioSource)
		{
			audioSource.clip = audioClip;
			audioSource.volume = volume.GetRandom();
			audioSource.pitch = pitch.GetRandom();
			audioSource.maxDistance = maxDistance;

			audioSource.Play();
		}

		public override void PlayServer(GameObject audioSource)
		{
			int? clipNumber = AudioManager.Instance.AudioClips.GetClipNumberInArray(audioClip);
			if (clipNumber == null) return;

			PlayAudioMessage.PlaySound(clipNumber.Value, volume.GetRandom(), pitch.GetRandom(), maxDistance, audioSource);
		}

		private void OnEnable()
		{
			if (animationCurve == null)
			{
				animationCurve = new AnimationCurve();
				animationCurve.AddKey(0, 1);
				animationCurve.AddKey(maxDistance, 0);
			}
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