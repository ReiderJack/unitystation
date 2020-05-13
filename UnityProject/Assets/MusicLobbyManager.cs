using System;
using System.Collections;
using System.Collections.Generic;
using SO.Audio;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class MusicLobbyManager : MonoBehaviour
{
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

	private  AudioSource currentLobbyAudioSource;

	private  bool isMusicMute;

	[Range(0f, 1f)]
	public  float MusicVolume = 1;

	[SerializeField]
	private  AudioClip[] musicClips;

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

	private void OnDisable()
	{
		//SOManagerMusic.Instance.currentLobbyAudioSource = null;
	}

	/// <summary>
	/// Plays a random music track.
	/// Using two diiferent ways to play tracks, some tracks are normal audio and some are tracker files played by sunvox.
	/// <returns>String[] that represents the picked song's name.</returns>
	/// </summary>
	public  String[] PlayRandomTrack()
	{
		StopMusic();
		String[] songInfo;

		// To make sure not to play the last song that just played,
		// every time a track is played, it's either a normal audio or track played by sunvox, alternatively.

			//Traditional music
		int randTrack = Random.Range(0, musicClips.Length);
		currentLobbyAudioSource.clip = musicClips[randTrack];
		var volume = MusicVolume;
		if (isMusicMute)
		{
			volume = 0f;
		}

		currentLobbyAudioSource.volume = volume;
		currentLobbyAudioSource.Play();
		songInfo = currentLobbyAudioSource.clip.name.Split('_'); // Spliting to get the song and artist name

		/*else
		{
			currentLobbyAudioSource = null;
			//Tracker music
			var trackerMusic = new[]
			{
				"Spaceman_HERB.xm",
				"Echo sound_4mat.xm",
				"Tintin on the Moon_Jeroen Tel.xm"
			};
			var songPicked = trackerMusic.Wrap(Random.Range(1, 100));
			var vol = 255 * MusicVolume;

			if (IsMusicMute)
			{
				vol = 0f;
			}

			Synth.Instance.PlayMusic(songPicked, false, (byte) (int) vol);
			songPicked = songPicked.Split('.')[0]; // Throwing away the .xm extension in the string
			songInfo = songPicked.Split('_'); // Spliting to get the song and artist name
		}*/

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

	/// <summary>
	/// Checks if music in lobby is being played or not.
	/// <returns> true if music is being played.</returns>
	/// </summary>
	public bool isLobbyMusicPlaying()
	{
		// Checks if an audiosource or a track by sunvox is being played(Since there are two diiferent ways to play tracks)
		if (currentLobbyAudioSource != null && currentLobbyAudioSource.isPlaying ||
		    !(SunVox.sv_end_of_song((int) Slot.Music) == 1))
			return true;

		return false;
	}
}
