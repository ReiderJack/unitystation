using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Projectiles.Behaviours
{

	public class BulletRangeLimited : MonoBehaviour
	{
		public float maxBulletDistance;

		protected Rigidbody2D rigidBody;
		private BulletBehaviour bulletBehaviour;

		private void Awake()
		{
			rigidBody = GetComponentInChildren<Rigidbody2D>();

			bulletBehaviour = GetComponentInChildren<BulletBehaviour>();
		}

		public void StartCountingTiles()
		{
			StartCoroutine(countTiles());
		}
		public IEnumerator countTiles()
		{
			float time = maxBulletDistance / bulletBehaviour.BulletVelocity;
			yield return WaitFor.Seconds(time);
			//Begin despawn
			DespawnThis();
		}

		protected virtual void DespawnThis()
		{
			rigidBody.velocity = Vector2.zero;
			Despawn.ClientSingle(gameObject);
		}
	}
}