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
		/*public int ComponentLocation;
		public uint NetObject;
		public Type ComponentType;

		public static readonly Dictionary<ushort, Type> componentIDToComponentType = new Dictionary<ushort, Type>(); //These are useful
		public static readonly Dictionary<Type, ushort> componentTypeToComponentID = new Dictionary<Type, ushort>();*/

		/*static PlaySoundMessageV2()
		{
			//initialize id mappings
			var alphabeticalComponentTypes =
				typeof(SoundListEvent).Assembly.GetTypes()
					.Where(type => typeof(SoundListEvent).IsAssignableFrom(type))
					.OrderBy(type => type.FullName);
			ushort i = 0;
			foreach (var componentType in alphabeticalComponentTypes)
			{
				componentIDToComponentType.Add(i, componentType);
				componentTypeToComponentID.Add(componentType, i);
				i++;
			}

		}*/

		public override void Process()
		{
			//LoadNetworkObject(NetObject);

			//SoundSingleton.Instance.
		}
	}
}