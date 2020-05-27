using System.Collections;
using UnityEngine;
using Utility = UnityEngine.Networking.Utility;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SO.Audio
{
	public class PlaySoundMessageV2 : ServerMessage
	{

		public int SoundPositionInList;
		public float Volume;
		public float Pitch;

		public Vector3 Position;
		public override void Process()
		{
			var auidoSourceToPlay = new AudioSource()
			{
				clip = SoundSingleton.Instance.audioClips[SoundPositionInList],
				volume = Volume,
				pitch = Pitch
			};

			AudioSourcePool.PlayAtPosition(Position,auidoSourceToPlay,Pitch,Volume);
		}

		public static PlaySoundMessageV2 Send( GameObject recipient, Vector3 pos,  int audioClip, float pitch, float volume, GameObject sourceObj = null )
		{

			var netId = NetId.Empty;
			if (sourceObj != null)
			{
				var netB = sourceObj.GetComponent<NetworkBehaviour>();
				if (netB != null)
				{
					netId = netB.netId;
				}
			}

			PlaySoundMessageV2 msg = new PlaySoundMessageV2
			{
				SoundPositionInList = audioClip,
				Volume = volume,
				Pitch = pitch,
				Position = pos
			};

			msg.SendTo(recipient);

			return msg;
		}
	}
}