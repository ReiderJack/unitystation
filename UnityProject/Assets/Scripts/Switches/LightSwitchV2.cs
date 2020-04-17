﻿using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

	public class LightSwitchV2 : NetworkBehaviour, ICheckedInteractable<HandApply>
	{
		public List<LightSourceV2> listLightSources;

		public APC relatedApc;

		public Action<bool> switchTriggerEvent;

		[SyncVar(hook = nameof(SyncState))]
		public bool isOn = true;
		private void Awake()
		{
			foreach (var lightSource in listLightSources)
			{
				if(lightSource != null)
					lightSource.SubscribeToSwitch(ref switchTriggerEvent);
			}
		}

		public bool WillInteract(HandApply interaction, NetworkSide side)
		{
			if (!DefaultWillInteract.Default(interaction, side)) return false;
			if (interaction.HandObject != null && interaction.Intent == Intent.Harm) return false;
			return true;
		}

		public void ServerPerformInteraction(HandApply interaction)
		{
			Debug.Log("Switch Pressed");
			ServerChangeState(!isOn);

		}

		private void SyncState(bool oldState, bool newState)
		{
			isOn = newState;
		}

		[Server]
		public void ServerChangeState(bool newState)
		{
			isOn = newState;
			switchTriggerEvent?.Invoke(isOn);
		}
		void OnDrawGizmosSelected()
		{
			var sprite = GetComponentInChildren<SpriteRenderer>();
			if (sprite == null)
				return;

			//Highlighting all controlled lightSources
			Gizmos.color = new Color(1, 1, 0, 1);
			for (int i = 0; i < listLightSources.Count; i++)
			{
				var lightSource = listLightSources[i];
				if(lightSource == null) continue;
				Gizmos.DrawLine(sprite.transform.position, lightSource.transform.position);
				Gizmos.DrawSphere(lightSource.transform.position, 0.25f);
			}
		}

	}