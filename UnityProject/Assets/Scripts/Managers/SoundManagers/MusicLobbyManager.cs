using System;
using SO.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicLobbyManager : MonoBehaviour
{
	private static MusicLobbyManager musicLobbyManager;
	public static MusicLobbyManager Instance
	{
		get
		{
			if (musicLobbyManager == null)
			{
				musicLobbyManager = FindObjectOfType<MusicLobbyManager>();
			}

			return musicLobbyManager;
		}
	}

	public AudioMixerGroup MusicMixer = null;

	[SerializeField]
	private SongTracker songTracker = null;

	/// <summary>
	/// For controlling the song play list. Includes random shuffle and auto play
	/// </summary>
	public SongTracker SongTracker => musicLobbyManager.songTracker;

	[SerializeField]
	private Slider musicLobbySlider = null;

	[SerializeField]
	private  AudioClipsList musicClips = null;

	private  bool isMusicMute = false;

	private AudioSource currentLobbyAudioSource;

	private void OnEnable()
	{
		if (currentLobbyAudioSource == null)
		{
			currentLobbyAudioSource = GetComponent<AudioSource>();
		}

		if (PlayerPrefs.HasKey(PlayerPrefKeys.MusicLobbyVolumeKey))
		{
			musicLobbySlider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.MusicLobbyVolumeKey);
		}

		if (PlayerPrefs.HasKey(PlayerPrefKeys.MuteMusic))
		{
			isMusicMute = PlayerPrefs.GetInt(PlayerPrefKeys.MuteMusic) == 0;
		}
	}

	public String[] PlayRandomTrack()
	{
		StopMusic();

		currentLobbyAudioSource.mute = isMusicMute;
		currentLobbyAudioSource.clip = musicClips.GetRandomClip();
		currentLobbyAudioSource.outputAudioMixerGroup = MusicMixer;
		currentLobbyAudioSource.volume = musicLobbySlider.value;
		currentLobbyAudioSource.Play();

		return currentLobbyAudioSource.clip.name.Split('_');
	}

	public void ToggleMusicMute(bool mute)
	{
		isMusicMute = mute;

		currentLobbyAudioSource.mute = mute;

		/*if (mute)
		{
			Synth.Instance.SetMusicVolume(Byte.MinValue);
		}
		else
		{
			var vol = 255 * musicLobbySlider.value;
			Synth.Instance.SetMusicVolume((byte) (int) vol);
		}*/
	}

	public void StopMusic()
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

	public void OnSliderChanged()
	{
		var sliderValue = musicLobbySlider.value;
		currentLobbyAudioSource.volume = sliderValue;
		PlayerPrefs.SetFloat(PlayerPrefKeys.MusicLobbyVolumeKey, sliderValue);
		PlayerPrefs.Save();
	}
}
