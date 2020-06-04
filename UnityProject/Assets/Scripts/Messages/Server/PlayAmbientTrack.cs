using System.Collections;
using Audio.Managers;
using UnityEngine;

/// <summary>
///     Message that tells client to play an ambient track
/// </summary>
public class PlayAmbientTrack : ServerMessage
{
	public string TrackName;

	public override void Process()
	{
		SoundAmbientManager.PlayAmbience(TrackName);
	}

	public static PlayAmbientTrack Send(GameObject recipient, string trackName)
	{
		PlayAmbientTrack msg = new PlayAmbientTrack
		{
			TrackName = trackName,
		};

		msg.SendTo(recipient);
		return msg;
	}
}