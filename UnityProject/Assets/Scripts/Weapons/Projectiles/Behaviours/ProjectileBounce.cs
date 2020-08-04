﻿using Container.HitConditions;
using UnityEngine;

namespace Weapons.Projectiles.Behaviours
{
	public class ProjectileBounce : MonoBehaviour, IOnShoot, IOnHitInteractTile
	{
		private Bullet bullet;

		private Vector2 direction;
		private GameObject shooter;
		private Gun weapon;
		private BodyPartType targetZone;

		[SerializeField] private HitInteractTileCondition[] hitInteractTileConditions;

		[SerializeField] private int maxHitCount = 4;
		private int currentCount = 0;

		private void Awake()
		{
			bullet = GetComponent<Bullet>();
		}

		public void OnShoot(Vector2 direction, GameObject shooter, Gun weapon, BodyPartType targetZone = BodyPartType.Chest)
		{
			this.direction = direction;
			this.shooter = shooter;
			this.weapon = weapon;
			this.targetZone = targetZone;
		}

		public bool Interact(RaycastHit2D hit, InteractableTiles interactableTiles, Vector3 worldPosition)
		{
			if (CheckConditions(hit, interactableTiles, worldPosition) == false) return true;

			RotateBullet(GetNewDirection(hit));

			return IsCountReached();
		}

		private bool CheckConditions(RaycastHit2D hit, InteractableTiles interactableTiles, Vector3 worldPosition)
		{
			bool isHit = false;
			foreach (var condition in hitInteractTileConditions)
			{
				if (condition.CheckCondition(hit, interactableTiles, worldPosition))
				{
					isHit = true;
				}
			}

			return isHit;
		}

		private void RotateBullet(Vector2 newDirection)
		{
			bullet.Shoot(newDirection * 2f, shooter, weapon, targetZone);
		}

		private Vector2 GetNewDirection(RaycastHit2D hit)
		{
			var normal = hit.normal;
			var newDirection = direction - 2 * (direction * normal) * normal;
			return newDirection;
		}

		private bool IsCountReached()
		{
			currentCount++;
			if (currentCount < maxHitCount) return false;
			currentCount = 0;
			return true;
		}

		private void OnDisable()
		{
			direction = Vector2.zero;
			shooter = null;
			weapon = null;
			targetZone = BodyPartType.None;
			currentCount = 0;
		}
	}
}