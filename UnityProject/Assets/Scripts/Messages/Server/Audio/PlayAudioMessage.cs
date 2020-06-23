using Audio;
using Mirror;
using UnityEngine;

namespace Messages.Server.Audio
{
	public class PlayAudioMessage : ServerMessage
	{
		public int ClipNumber;
		public float Volume;
		public float Pitch;
		public float MaxDistance;

		public uint TargetNetId;

		public override void Process()
		{
			AudioManager.Play(ClipNumber, Volume, Pitch, MaxDistance, TargetNetId);
		}

		public static PlayAudioMessage PlaySound(int clipNumber, float volume, float pitch, float maxDistance, GameObject audioSource)
		{
			var netId = NetId.Empty;
			var position = new Vector3();
			if (audioSource != null)
			{
				position = audioSource.gameObject.transform.position;
				var netB = audioSource.GetComponent<NetworkBehaviour>();
				if (netB != null)
				{
					netId = netB.netId;
				}

			}

			PlayAudioMessage msg = new PlayAudioMessage
			{
				ClipNumber = clipNumber,
				Volume = volume,
				Pitch = pitch,
				MaxDistance = maxDistance,
				TargetNetId = netId
			};

			msg.SendToNearbyPlayers(position, maxDistance);
			return msg;
		}
	}
}