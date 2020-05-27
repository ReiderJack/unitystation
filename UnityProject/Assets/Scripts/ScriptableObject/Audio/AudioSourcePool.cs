using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace SO.Audio
{
	public class AudioSourcePool : MonoBehaviour
	{
		private static AudioSourcePool audioSourcePool;
		public static AudioSourcePool Instance
		{
			get
			{
				if (!audioSourcePool)
				{
					audioSourcePool = FindObjectOfType<AudioSourcePool>();
				}

				return audioSourcePool;
			}
		}

		[SerializeField] private GameObject soundSpawnPrefab = null;
		public List<SoundSpawn> pooledSources = new List<SoundSpawn>();

		[SerializeField]
		private AudioMixer audioMixer;

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += OnSceneChange;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnSceneChange;
		}

		void OnSceneChange(Scene oldScene, Scene newScene)
		{
			ReinitSoundPool();
		}

		private void ReinitSoundPool()
		{
			for (int i = Instance.pooledSources.Count - 1; i > 0; i--)
			{
				if (Instance.pooledSources[i] != null)
				{
					Destroy(Instance.pooledSources[i].gameObject);
				}
			}

			Instance.pooledSources.Clear();

			// Cache some pooled sources:
			for (int i = 0; i < 20; i++)
			{
				var soundObj = Instantiate(soundSpawnPrefab, transform);
				pooledSources.Add(soundObj.GetComponent<SoundSpawn>());
			}
		}

		public SoundSpawn GetSourceFromPool(AudioSource sourceToCopy)
		{
			for (int i = pooledSources.Count - 1; i > 0; i--)
			{
				if (pooledSources[i] != null && pooledSources[i].gameObject != null
				                             && !pooledSources[i].isPlaying)
				{
					pooledSources[i].isPlaying = true;
					CopySource(pooledSources[i].audioSource, sourceToCopy);
					return pooledSources[i];
				}
			}

			var soundObj = Instantiate(soundSpawnPrefab, transform);
			var source = soundObj.GetComponent<SoundSpawn>();
			pooledSources.Add(source);
			source.isPlaying = true;
			CopySource(source.audioSource, sourceToCopy);
			return source;
		}

		private void CopySource(AudioSource newSource, AudioSource sourceToCopy)
		{
			newSource.clip = sourceToCopy.clip;
			newSource.loop = sourceToCopy.loop;
			newSource.pitch = sourceToCopy.pitch;
			newSource.mute = sourceToCopy.mute;
			newSource.spatialize = sourceToCopy.spatialize;
			newSource.spread = sourceToCopy.spread;
			newSource.volume = sourceToCopy.volume;
			newSource.bypassEffects = sourceToCopy.bypassEffects;
			newSource.dopplerLevel = sourceToCopy.dopplerLevel;
			newSource.maxDistance = sourceToCopy.maxDistance;
			newSource.minDistance = sourceToCopy.minDistance;
			newSource.panStereo = sourceToCopy.panStereo;
			newSource.rolloffMode = sourceToCopy.rolloffMode;
			newSource.spatialBlend = sourceToCopy.spatialBlend;
			newSource.bypassListenerEffects = sourceToCopy.bypassListenerEffects;
			newSource.bypassReverbZones = sourceToCopy.bypassReverbZones;
			newSource.reverbZoneMix = sourceToCopy.reverbZoneMix;
			newSource.spatializePostEffects = sourceToCopy.spatializePostEffects;
			newSource.outputAudioMixerGroup = sourceToCopy.outputAudioMixerGroup;
			newSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff,
				sourceToCopy.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
			newSource.SetCustomCurve(AudioSourceCurveType.Spread,
				sourceToCopy.GetCustomCurve(AudioSourceCurveType.Spread));
			newSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend,
				sourceToCopy.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
			newSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix,
				sourceToCopy.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
		}

		public static void PlayAtPosition(Vector3 worldPos, AudioSource audioClip, float pitch = -1, float volume = 1,
											bool polyphonic = false, bool isGlobal = true, uint netId = NetId.Empty)
		{
			var sound = AudioSourcePool.Instance.GetSourceFromPool(audioClip);

			if (pitch > 0)
			{
				sound.audioSource.pitch = pitch;
			}

			sound.audioSource.volume = volume;
			if (netId != NetId.Empty)
			{
				if (NetworkIdentity.spawned.ContainsKey(netId))
				{
					sound.transform.parent = NetworkIdentity.spawned[netId].transform;
					sound.transform.localPosition = Vector3.zero;
				}
				else
				{
					sound.transform.parent = Instance.transform;
					sound.transform.position = worldPos;
				}
			}
			else
			{
				sound.transform.parent = Instance.transform;
				sound.transform.position = worldPos;
			}

			Instance.PlaySource(sound, polyphonic, isGlobal);
		}

		private void PlaySource(SoundSpawn source, bool polyphonic = false, bool Global = true)
		{
			source.audioSource.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;
			if (polyphonic)
			{
				source.PlayOneShot();
			}
			else
			{
				source.PlayNormally();
			}
		}
	}
}