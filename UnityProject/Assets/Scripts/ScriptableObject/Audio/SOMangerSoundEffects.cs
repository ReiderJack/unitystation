using UnityEngine;

namespace SO.Audio
{
	[CreateAssetMenu(fileName = "SOManagerSoundEffects", menuName = "Singleton/SOManagerSoundEffects")]
	public class SOManagerSoundEffects : SingletonScriptableObject<SOManagerSoundEffects>
	{
		[SerializeField] private SoundEvent[] sounds;

		public void PlaySound(SoundEvent sound)
		{

		}
	}
}