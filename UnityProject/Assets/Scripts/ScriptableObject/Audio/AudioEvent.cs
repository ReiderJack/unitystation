using UnityEngine;

namespace Audio.Containers
{
	public abstract class AudioEvent : ScriptableObject
	{
		public abstract void Play(AudioSource audioSource);
	}
}