using System;
using System.Collections;
using System.Collections.Generic;
using SO.Audio;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class MusicLobbyManager : MonoBehaviour
{
	[SerializeField]
	private  AudioClip[] musicClips;
	private  AudioSource currentLobbyAudioSource;

	private  bool isMusicMute;

	private static MusicLobbyManager musicLobbyManager;
	public static MusicLobbyManager Instance
	{
		get
		{
			if (!musicLobbyManager)
			{
				musicLobbyManager = FindObjectOfType<MusicLobbyManager>();
			}

			return musicLobbyManager;
		}
	}

	public AudioMixerGroup MusicMixer;

	[Range(0f, 1f)]
	public  float MusicVolume = 1;

	private void OnEnable()
	{
		if (PlayerPrefs.HasKey(PlayerPrefKeys.MuteMusic))
		{
			isMusicMute = PlayerPrefs.GetInt(PlayerPrefKeys.MuteMusic) == 0;
		}

		if (currentLobbyAudioSource == null)
		{
			currentLobbyAudioSource = GetComponent<AudioSource>();
		}
	}

	public  String[] PlayRandomTrack()
	{
		StopMusic();
		String[] songInfo;

		int randTrack = Random.Range(0, musicClips.Length);
		currentLobbyAudioSource.clip = musicClips[randTrack];
		var volume = MusicVolume;
		if (isMusicMute)
		{
			volume = 0f;
		}

		currentLobbyAudioSource.outputAudioMixerGroup = MusicMixer;
		currentLobbyAudioSource.volume = volume;
		currentLobbyAudioSource.Play();
		songInfo = currentLobbyAudioSource.clip.name.Split('_');

		return songInfo;
	}

	public  void ToggleMusicMute(bool mute)
	{
		isMusicMute = mute;

		currentLobbyAudioSource.mute = mute;

		if (mute)
		{
			Synth.Instance.SetMusicVolume(Byte.MinValue);
		}
		else
		{
			var vol = 255 * MusicVolume;
			Synth.Instance.SetMusicVolume((byte) (int) vol);
		}
	}

	public  void StopMusic()
	{
		currentLobbyAudioSource.Stop();

		Synth.Instance.StopMusic();
	}

	public bool isLobbyMusicPlaying()
	{
		if (currentLobbyAudioSource != null && currentLobbyAudioSource.isPlaying)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
