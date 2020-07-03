using UnityEngine;

namespace Weapons.Projectiles.Behaviours
{
	public interface IBulletBehaviour
	{
		void GetShooterInfo(GameObject shooter, Gun weapon, bool isSuicide);
		void HandleCollisionEnter2D(Collision2D coll);
		void HandleTriggerEnter2D(Collider2D coll);
	}
}