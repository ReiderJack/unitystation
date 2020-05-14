using System;
using SO.Audio;
using UnityEngine;
using UnityEngine.Audio;

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

	[Range(0f, 1f)]
	private float musicVolume = 1;

	public float MusicVolume
	{
		get => musicVolume;
		set
		{
			musicVolume = value;
			currentLobbyAudioSource.volume = value;
		}
	}

	[SerializeField]
	private SongTracker songTracker = null;

	/// <summary>
	/// For controlling the song play list. Includes random shuffle and auto play
	/// </summary>
	public SongTracker SongTracker => musicLobbyManager.songTracker;

	[SerializeField]
	private  AudioClipsList musicClips;

	private  bool isMusicMute;

	private  AudioSource currentLobbyAudioSource;

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

		currentLobbyAudioSource.clip = musicClips.GetRandomClip();
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
