using Audio.Managers;
using UnityEngine;

/// <summary>
///     Message that tells client to play an ambient track
/// </summary>
public class PlayAmbientTrack : ServerMessage
{
	public string TrackName;
	public bool isTrackLooped;

	public override void Process()
	{
		SoundAmbientManager.StopAllTracks();
		SoundAmbientManager.TryPlayTrack(TrackName,isTrackLooped);
	}

	public static PlayAmbientTrack Send(GameObject recipient, string trackName, bool isLooped = false)
	{
		PlayAmbientTrack msg = new PlayAmbientTrack
		{
			isTrackLooped = isLooped,
			TrackName = trackName
		};

		msg.SendTo(recipient);
		return msg;
	}
}