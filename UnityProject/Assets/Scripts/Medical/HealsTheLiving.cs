﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
 using Health;
 using UnityEngine;
using Mirror;

/// <summary>
/// Component which allows this object to be applied to a living thing, healing it.
/// </summary>
[RequireComponent(typeof(Stackable))]
public class HealsTheLiving : MonoBehaviour, ICheckedInteractable<HandApply>
{
	private static readonly StandardProgressActionConfig ProgressConfig =
		new StandardProgressActionConfig(StandardProgressActionType.SelfHeal);

	public DamageType healType;
	private Stackable stackable;

	private void Awake()
	{
		stackable = GetComponent<Stackable>();
	}

	public bool WillInteract(HandApply interaction, NetworkSide side)
	{
		if (!DefaultWillInteract.Default(interaction, side)) return false;
		//can only be applied to LHB
		if (!Validations.HasComponent<HealthSystem>(interaction.TargetObject)) return false;
		return true;
	}

	public void ServerPerformInteraction(HandApply interaction)
	{
		var LHB = interaction.TargetObject.GetComponent<HealthSystem>();
		if (LHB.IsDead)
		{
			return;
		}
		var targetBodyPart = LHB.FindBodyPart(interaction.TargetBodyPart);
		if (targetBodyPart.GetDamageValue(healType) > 0)
		{
			if (interaction.TargetObject != interaction.Performer)
			{
				ServerApplyHeal(targetBodyPart);
			}
			else
			{
				ServerSelfHeal(interaction.Performer, targetBodyPart);
			}
		}
		else
		{
			Chat.AddExamineMsgFromServer(interaction.Performer, $"The {interaction.TargetBodyPart} does not need to be healed.");
		}
	}

	private void ServerApplyHeal(BodyPart targetBodyPart)
	{
		targetBodyPart.HealDamage(40, healType);
		stackable.ServerConsume(1);

		HealthBodyPartMessage.Send(targetBodyPart.gameObject, targetBodyPart.gameObject,
			targetBodyPart.bodyPartData.bodyPartType, targetBodyPart.BruteDamage,
			targetBodyPart.BurnDamage);
	}

	private void ServerSelfHeal(GameObject originator, BodyPart targetBodyPart)
	{
		void ProgressComplete()
		{
			ServerApplyHeal(targetBodyPart);
		}

		StandardProgressAction.Create(ProgressConfig, ProgressComplete)
			.ServerStartProgress(originator.RegisterTile(), 5f, originator);
	}
}