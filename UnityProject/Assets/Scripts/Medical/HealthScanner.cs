using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;

/// <summary>
/// Main health scanner interaction. Applying it to a living thing sends a request to the server to
/// tell us their health info.
/// </summary>
public class HealthScanner : MonoBehaviour, ICheckedInteractable<HandApply>
{
	public bool WillInteract(HandApply interaction, NetworkSide side)
	{
		if (!DefaultWillInteract.Default(interaction, side)) return false;
		//can only be applied to LHB
		if (!Validations.HasComponent<HealthSystem>(interaction.TargetObject)) return false;
		return true;
	}

	public void ServerPerformInteraction(HandApply interaction)
	{
		var livingHealth = interaction.TargetObject.GetComponent<HealthSystem>();
		string ToShow = (livingHealth.name + " is " + livingHealth.ConsciousState.ToString() + "\n"
		                 + "OverallHealth = " + livingHealth.OverallHealth.ToString() + " Blood level = " +
		                 livingHealth.bloodSystem.BloodLevel.ToString() + "\n"
		                 + "Blood levels = " + livingHealth.CalculateOverallBloodLossDamage() + "\n");
		string StringBuffer = "";
		float TotalBruteDamage = 0;
		float TotalBurnDamage = 0;
		foreach (BodyPart bodyPart in livingHealth.bodyParts)
		{
			StringBuffer += bodyPart.bodyPartData.bodyPartType.ToString() + "\t";
			StringBuffer += bodyPart.BruteDamage.ToString() + "\t";
			TotalBruteDamage += bodyPart.BruteDamage;
			StringBuffer += bodyPart.BurnDamage.ToString();
			TotalBurnDamage += bodyPart.BurnDamage;
			StringBuffer += "\n";
		}

		ToShow = ToShow + "Overall, Brute " + TotalBruteDamage.ToString() + " Burn " + TotalBurnDamage.ToString() + " Toxin " + livingHealth.bloodSystem.ToxinLevel +
		         " OxyLoss " + livingHealth.bloodSystem.OxygenDamage.ToString() + "\n" + "Body Part, Brute, Burn \n" +
		         StringBuffer;
		if (livingHealth.cloningDamage > 0)
		{
			ToShow += $"Cellular Damage Level: {livingHealth.cloningDamage}";
		}

		Chat.AddExamineMsgFromServer(interaction.Performer, ToShow);
	}
}