using Mirror;
using SO.Audio;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to display the information of the song being played in the lobby screen.
/// </summary>
public class SongTracker : MonoBehaviour
{
	[SerializeField] private Text trackName = null;
	[SerializeField] private Text artist = null;
	[SerializeField] private Image speakerImage = null;
	[SerializeField] private Sprite speakerOn = null;
	[SerializeField] private Sprite speakerOff = null;
	[SerializeField] private Color onColor = new Color(178, 194, 204); // Pale Blue
	[SerializeField] private Color offColor = new Color(176, 176, 176); // Grey

	private float timeBetweenSongs = 2f;
	private float currentWaitTime = 0f;

	/// <summary>
	/// If true the SongTracker will continue to play tracks one after
	/// another in a random order
	/// </summary>
	public bool PlayingRandomPlayList { get; private set; }

	void Awake()
	{
		ToggleUI(false);
		if (!PlayerPrefs.HasKey(PlayerPrefKeys.MuteMusic))
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.MuteMusic, 1);
			PlayerPrefs.Save();
		}

	}

	void Start()
	{
		DetermineMuteState();
	}

	void Update()
	{
		if (!PlayingRandomPlayList || CustomNetworkManager.isHeadless) return;

		if (!MusicLobbyManager.Instance.isLobbyMusicPlaying())
		{
			currentWaitTime += Time.deltaTime;
			if (currentWaitTime >= timeBetweenSongs)
			{
				currentWaitTime = 0f;
				PlayRandomTrack();
			}
			DetermineMuteState();
		}
	}

	public void StartPlayingRandomPlaylist()
	{
		if(CustomNetworkManager.isHeadless) return;

		PlayingRandomPlayList = true;
		PlayRandomTrack();
		ToggleUI(true);
	}

	public void Stop()
	{
		PlayingRandomPlayList = false;
		ToggleUI(false);
		MusicLobbyManager.Instance.StopMusic();
	}

	void ToggleUI(bool isActive)
	{
		trackName.gameObject.SetActive(isActive);
		artist.gameObject.SetActive(isActive);
		speakerImage.gameObject.SetActive(isActive);
	}

	public void ToggleMusicMute()
	{
		var toggle = PlayerPrefs.GetInt(PlayerPrefKeys.MuteMusic);
		if (toggle == 0)
		{
			toggle = 1;
		}
		else
		{
			toggle = 0;
		}

		PlayerPrefs.SetInt(PlayerPrefKeys.MuteMusic, toggle);
		PlayerPrefs.Save();
		DetermineMuteState();

	}

	void DetermineMuteState()
	{
		var toggle = PlayerPrefs.GetInt(PlayerPrefKeys.MuteMusic);
		switch (toggle)
		{
			case 0:
				speakerImage.sprite = speakerOff;
				speakerImage.color = offColor;
				MusicLobbyManager.Instance.ToggleMusicMute(true);
				break;
			case 1:
				speakerImage.sprite = speakerOn;
				speakerImage.color = onColor;
				MusicLobbyManager.Instance.ToggleMusicMute(false);
				break;
		}
	}

	public void PlayRandomTrack()
	{
		if(CustomNetworkManager.isHeadless) return;

		var songInfo = MusicLobbyManager.Instance.PlayRandomTrack();
		trackName.text = songInfo[0];
		// If the name of the artist is included, add it as well
		if (songInfo.Length == 2)
		{
			artist.text = songInfo[1];
		}
		else
		{
			artist.text = "";
		}
	}

	public void OnSliderChanged(float sliderValue)
	{
		MusicLobbyManager.Instance.MusicVolume = sliderValue;
	}
}
