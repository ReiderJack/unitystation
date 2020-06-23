using UnityEngine;

namespace Messages.Server
{
	public class ShakeCameraMessage : ServerMessage
	{
		public byte Intensity;
		public byte Length;

		public override void Process()
		{
			float intensity = Mathf.Clamp(Intensity/(float)byte.MaxValue, 0.01f, 10f);
			Camera2DFollow.followControl.Shake(intensity, Length);
		}

		public static ServerMessage SendShakeGround(Vector3 position, int shakeRange, byte shakeIntensity, byte length)
		{
			ShakeCameraMessage message = new ShakeCameraMessage()
			{
				Intensity = shakeIntensity,
				Length = length
			};
			message.SendToNearbyPlayers(position,shakeRange);
			return message;
		}
	}
}