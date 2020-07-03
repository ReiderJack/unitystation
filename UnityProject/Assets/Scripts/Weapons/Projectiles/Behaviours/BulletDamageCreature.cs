using System;
using UnityEngine;

namespace Weapons.Projectiles.Behaviours
{
	public class BulletDamageCreature : MonoBehaviour, IBulletBehaviour
	{
		private BodyPartType bodyAim;
		[Range(0, 100)]
		public float damage = 25;
		private GameObject Shooter;
		private Gun Weapon;
		public DamageType damageType;
		public AttackType attackType = AttackType.Bullet;
		private bool IsSuicide = false;

		public void Awake()
		{
			GetComponent<BulletBehaviour>().OnStartShoot += GetShooterInfo;
		}

		public void GetShooterInfo(GameObject shooter, Gun weapon, bool isSuicide)
		{
			Shooter = shooter;
			Weapon = weapon;
			this.IsSuicide = isSuicide;
		}

		public void HandleCollisionEnter2D(Collision2D coll)
		{
		}

		public void HandleTriggerEnter2D(Collider2D coll)
		{
			//only harm others if it's not a suicide
			if (coll.gameObject == Shooter && !IsSuicide)
			{
				return;
			}

			//only harm others if it's not a suicide
			if (coll.gameObject != Shooter && IsSuicide)
			{
				return;
			}

			var livingHealth = coll.GetComponent<LivingHealthBehaviour>();

			//damage human if there is one
			if (livingHealth == null || livingHealth.IsDead)
			{
				return;
			}

			// Trigger for things like stuns
			GetComponent<BulletHitTrigger>()?.BulletHitInteract(coll.gameObject);

			var aim = bodyAim;
			livingHealth.ApplyDamageToBodypart(gameObject, damage, attackType, damageType, aim);
			Chat.AddAttackMsgToChat(Shooter, coll.gameObject, aim, Weapon.gameObject);
			Logger.LogTraceFormat("Hit {0} for {1} with HealthBehaviour! bullet absorbed", Category.Firearms, livingHealth.gameObject.name, damage);
		}
	}
}