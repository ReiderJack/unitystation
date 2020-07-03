using UnityEngine;

namespace Weapons.Projectiles.Behaviours
{
	public class BulletDamageIntegrity : MonoBehaviour, IBulletBehaviour
	{
		[Range(0, 100)]
		public float damage = 25;

		private GameObject Shooter;
		private Gun Weapon;
		public DamageType damageType;
		public AttackType attackType = AttackType.Bullet;

		public void GetShooterInfo(GameObject shooter, Gun weapon, bool isSuicide)
		{
			Shooter = shooter;
			Weapon = weapon;
		}

		public void HandleCollisionEnter2D(Collision2D coll)
		{
		}

		public void HandleTriggerEnter2D(Collider2D coll)
		{
			var integrity = coll.GetComponent<Integrity>();
			if (integrity != null)
			{
				//damage object
				integrity.ApplyDamage(damage, attackType, damageType);
				Chat.AddAttackMsgToChat(Shooter, coll.gameObject, BodyPartType.None, Weapon.gameObject);
				Logger.LogTraceFormat("Hit {0} for {1} with HealthBehaviour! bullet absorbed", Category.Firearms, integrity.gameObject.name, damage);
			}
		}
	}
}