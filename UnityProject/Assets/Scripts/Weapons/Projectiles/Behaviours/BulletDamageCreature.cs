using UnityEngine;

namespace Weapons.Projectiles.Behaviours
{
	public class BulletDamageCreature : MonoBehaviour, IBulletBehaviour
	{
		private BodyPartType bodyAim;
		[Range(0, 100)]
		public float damage = 25;
		private GameObject shooter;
		protected Gun weapon;
		public DamageType damageType;
		public AttackType attackType = AttackType.Bullet;
		private bool isSuicide = false;

		public void HandleCollisionEnter2D(Collision2D coll)
		{
		}

		public void HandleTriggerEnter2D(Collider2D coll)
		{
			//only harm others if it's not a suicide
			if (coll.gameObject == shooter)
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

			var aim = isSuicide ? bodyAim : bodyAim.Randomize();
			livingHealth.ApplyDamageToBodypart(shooter, damage, attackType, damageType, aim);
			//Chat.AddAttackMsgToChat(shooter, coll.gameObject, aim, weapon.gameObject);
			Logger.LogTraceFormat("Hit {0} for {1} with HealthBehaviour! bullet absorbed", Category.Firearms, livingHealth.gameObject.name, damage);
		}
	}
}