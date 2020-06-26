﻿using System.Collections;
using Health;
using UnityEngine;
using Mirror;

/// <summary>
///     Tells client to update conscious state
/// </summary>
public class HealthConsciousMessage : ServerMessage
{
	public uint EntityToUpdate;
	public ConsciousState ConsciousState;

	public override void Process()
	{
		LoadNetworkObject(EntityToUpdate);
		if (NetworkObject == null)
		{
			return;
		}

		var healthBehaviour = NetworkObject.GetComponent<HealthSystem>();

		if (healthBehaviour != null)
		{
			healthBehaviour.UpdateClientConsciousState(ConsciousState);
		}
		else
		{
			Logger.Log($"Living health behaviour not found for {NetworkObject.ExpensiveName()} skipping conscious state update", Category.Health);
		}
	}

	public static HealthConsciousMessage Send(GameObject recipient, GameObject entityToUpdate, ConsciousState consciousState)
	{
		HealthConsciousMessage msg = new HealthConsciousMessage
		{
			EntityToUpdate = entityToUpdate.GetComponent<NetworkIdentity>().netId,
			ConsciousState = consciousState
		};
		msg.SendTo(recipient);
		return msg;
	}

	public static HealthConsciousMessage SendToAll(GameObject entityToUpdate, ConsciousState consciousState)
	{
		HealthConsciousMessage msg = new HealthConsciousMessage
		{
			EntityToUpdate = entityToUpdate.GetComponent<NetworkIdentity>().netId,
			ConsciousState = consciousState
		};
		msg.SendToAll();
		return msg;
	}
}