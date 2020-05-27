using System.Collections;
using System.Collections.Generic;
using SO.Audio;
using UnityEngine;

public class GunPKA : Gun
{

	bool allowRecharge = true;
	public float rechargeTime = 2.0f;

	public SoundListEvent soundListEvent;
	public override void ServerPerformInteraction(AimApply interaction)
	{
		ServerShoot(interaction.Performer, interaction.TargetVector.normalized, UIManager.DamageZone, false);
		soundListEvent.PlayAtPosition(interaction.Performer,gameObject.AssumedWorldPosServer());
		if (allowRecharge)
		{
			StartCoroutine(StartCooldown());
		}
	}
	private IEnumerator StartCooldown()
	{
		allowRecharge = false;
		yield return WaitFor.Seconds(rechargeTime);
		CurrentMagazine.ExpendAmmo(-1);
		SoundManager.PlayNetworkedAtPos("ReloadKinetic", gameObject.AssumedWorldPosServer(), sourceObj: serverHolder);
		allowRecharge = true;
	}
}
