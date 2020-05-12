using UnityEngine;

namespace SO.Audio
{
	public abstract class SoundEvent : UnityEngine.ScriptableObject
	{
		public abstract void Play(AudioSource source);
	}
}