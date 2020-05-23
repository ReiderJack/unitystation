using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SO.Audio
{
	[CreateAssetMenu(fileName = "SoundSingleton", menuName = "Singleton/SoundSingleton")]
	public class SoundSingleton : SingletonScriptableObject<SoundSingleton>
	{
		public List<SoundListEvent> uIActions = new List<SoundListEvent>();

		public static Dictionary<ushort, SoundListEvent> IDtoSound = new Dictionary<ushort, SoundListEvent>();
		public static Dictionary<SoundListEvent, ushort> SoundToID = new Dictionary<SoundListEvent, ushort>();

		private static bool Initialised = false;

		void OnEnable()
		{
			Setup();
		}

		void Setup()
		{
			ushort ID = 1;
			var alphabeticaluIActions= uIActions.OrderBy(X => X.name);

			foreach (var action in alphabeticaluIActions)
			{
				IDtoSound[ID] = action;
				SoundToID[action] = ID;
				ID++;
			}
			Initialised = true;
		}

		public SoundListEvent ReturnFromID(ushort ID)
		{
			if (!Initialised)
			{
				Setup();
			}

			if (IDtoSound.ContainsKey(ID))
			{
				return (IDtoSound[ID] as SoundListEvent);
			}
			return (null);
		}

		public void ActionCallServer(ushort ID, ConnectedPlayer SentByPlayer)
		{
			if (!Initialised)
			{
				Setup();
			}
			if (IDtoSound.ContainsKey(ID))
			{
				//IDtoSound[ID].SoundToID(SentByPlayer);
			}
		}

	}
}