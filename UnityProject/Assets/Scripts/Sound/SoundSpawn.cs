using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundSpawn : MonoBehaviour
{
	private static SoundSpawn soundSpawn;

	public static SoundSpawn Instance
	{
		get
		{
			if (soundSpawn == null)
			{
				soundSpawn = FindObjectOfType<SoundSpawn>();
			}

			return soundSpawn;
		}
	}

	private List<SoundSpawn> pooledSources = new List<SoundSpawn>();
	public AudioSource audioSource;
	//We need to handle this manually to prevent multiple requests grabbing sound pool items in the same frame
	public bool isPlaying = false;
	private float waitLead = 0;
	private bool monitor = false;

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

	public SoundSpawn GetSourceFromPool(AudioSource sourceToCopy)
	{
		for (int i = pooledSources.Count - 1; i > 0; i--)
		{
			if (pooledSources[i] != null
			    && pooledSources[i].gameObject != null
			    && !pooledSources[i].isPlaying)
			{
				pooledSources[i].isPlaying = true;
				CopySource(pooledSources[i].audioSource, sourceToCopy);
				return pooledSources[i];
			}
		}

		var source = GetComponent<SoundSpawn>();
		pooledSources.Add(source);
		source.isPlaying = true;
		CopySource(source.audioSource, sourceToCopy);
		return source;
	}

	public void PlayOneShot()
	{
		if (audioSource == null) return;
		audioSource.PlayOneShot(audioSource.clip);
		WaitForPlayToFinish();
	}

	public void PlayNormally()
	{
		if (audioSource == null) return;
		audioSource.Play();
		WaitForPlayToFinish();
	}

	void WaitForPlayToFinish()
	{
		waitLead = 0f;
		monitor = true;
	}

	public void Stop(AudioSource audioSource)
	{
		for (int i = Instance.pooledSources.Count - 1; i > 0; i--)
		{
			if (Instance.pooledSources[i] == null) continue;

			if (Instance.pooledSources[i].isPlaying
			    && Instance.pooledSources[i].audioSource.clip == audioSource.clip)
			{
				Instance.pooledSources[i].audioSource.Stop();
			}
		}
		audioSource.Stop();
	}

	private void OnEnable()
	{
		// Cache some pooled sources:
		for (int i = 0; i < 20; i++)
		{
			pooledSources.Add(this);
		}
		//ReinitSoundPool();
		//SceneManager.activeSceneChanged += OnSceneChange;
		UpdateManager.Add(CallbackType.UPDATE, UpdateMe);
	}

	private void OnDisable()
	{
		//SceneManager.activeSceneChanged -= OnSceneChange;
		UpdateManager.Remove(CallbackType.UPDATE, UpdateMe);
	}

	void OnSceneChange(Scene oldScene, Scene newScene)
	{
		ReinitSoundPool();
	}

	void ReinitSoundPool()
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
			pooledSources.Add(this);
		}
	}

	void UpdateMe()
	{
		if (!monitor || audioSource == null) return;

		waitLead += Time.deltaTime;
		if (waitLead > 0.2f)
		{
			if (!audioSource.isPlaying)
			{
				isPlaying = false;
				waitLead = 0f;
				monitor = false;
			}
		}
	}
}
