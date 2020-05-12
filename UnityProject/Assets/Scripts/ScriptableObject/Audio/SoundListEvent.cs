using UnityEngine;
using CustomVariables.Float;
using SO.Audio;
using UnityEngine.Audio;

namespace SO.Audio
{
	[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/Sounds/SoundList")]
	public class SoundListEvent : SoundEvent
	{
		[SerializeField]
		private AudioClip[] audioClips;

		[SerializeField]
		private FloatRanged volume = new FloatRanged();

		[SerializeField]
		[FloatRange(0, 2)]
		private FloatRanged pitch = new FloatRanged();

		public override void Play(AudioSource audioSource)
		{
			if (audioClips.Length == 0) return;

			audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
			var clip = audioSource.clip;
			audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
			audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
			audioSource.Play();
		}
	}
}