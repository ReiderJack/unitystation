using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Main behavior for a bullet, handles shooting and managing the trail rendering. Collision events are fired on
/// the child gameobject's BulletColliderBehavior and passed up to this component.
///
/// Note that the bullet prefab has this on the root transform, but the actual traveling projectile is in a
/// child transform. When shooting happens, the root transform remains still relative to its parent, but
/// the child transform is the one that actually moves.
///
/// This allows the trail to be relative to the matrix, so the trail still looks correct when the matrix is moving.
/// </summary>
public class BulletBehaviour : MonoBehaviour
{
	public float damage = 25;
	public bool isMiningBullet = false;
	/// <summary>
	/// Cached trailRenderer. Note that not all bullets have a trail, thus this can be null.
	/// </summary>
	private LocalTrailRenderer trailRenderer;
	/// <summary>
	/// Rigidbody on the child transform (the one that actually moves when a shot happens)
	/// </summary>
	private Rigidbody2D rigidBody;
	private bool isSuicide = false;

	public Action<GameObject,Gun,bool> OnStartShoot;
	[SerializeField]private float maxBulletDistance;
	[SerializeField]private bool isRangeLimited = false;

	private void Awake()
	{
		//Using Awake() instead of start because Start() doesn't seem to get called when this is instantiated
		if (trailRenderer == null)
		{
			trailRenderer = GetComponent<LocalTrailRenderer>();
		}

		if (rigidBody == null)
		{
			rigidBody = GetComponentInChildren<Rigidbody2D>();
		}
	}

	public Vector2 Direction { get; private set; }

	/// <summary>
	/// Shoot the controlledByPlayer
	/// </summary>
	/// <param name="controlledByPlayer">player doing the shooting</param>
	/// <param name="targetZone">body part being targeted</param>
	/// <param name="fromWeapon">Weapon the shot is being fired from</param>
	public void Suicide(GameObject controlledByPlayer, Gun fromWeapon, BodyPartType targetZone = BodyPartType.Chest) {
		isSuicide = true;
		StartShoot(Vector2.zero, controlledByPlayer, fromWeapon, targetZone);
	}

	/// <summary>
	/// Shoot in a direction
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="controlledByPlayer"></param>
	/// <param name="targetZone"></param>
	/// <param name="fromWeapon">Weapon the shot is being fired from</param>
	public virtual void Shoot(Vector2 dir, GameObject controlledByPlayer, Gun fromWeapon, BodyPartType targetZone = BodyPartType.Chest)
	{
		isSuicide = false;
		StartShoot(dir, controlledByPlayer, fromWeapon, targetZone);
		if (isRangeLimited)
		{
			StartCoroutine(CountTiles(fromWeapon.ProjectileVelocity));
		}
	}

	protected void StartShoot(Vector2 dir, GameObject controlledByPlayer, Gun fromWeapon, BodyPartType targetZone)
	{
		OnStartShoot?.Invoke(controlledByPlayer,fromWeapon,isSuicide);
		Direction = dir;

		transform.parent = controlledByPlayer.transform.parent;
		Vector3 startPos = new Vector3(dir.x, dir.y, transform.position.z) / 2;
		transform.position += startPos;
		rigidBody.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg, Vector3.forward);
		rigidBody.transform.localPosition = Vector3.zero;
		if (!isSuicide)
		{
			//TODO: Which is better? rigidBody.AddForce(dir.normalized * fromWeapon.ProjectileVelocity, ForceMode2D.Impulse);
			rigidBody.velocity = dir.normalized * fromWeapon.ProjectileVelocity;
		}
		else
		{
			rigidBody.velocity = Vector2.zero;
		}

		//tell our trail to start drawing if we have one
		if (trailRenderer != null)
		{
			trailRenderer.ShotStarted();
		}
	}

	private IEnumerator CountTiles(float bulletVelocity)
	{
		float time = maxBulletDistance / bulletVelocity;
		yield return WaitFor.Seconds(time);
		//Begin despawn
		DespawnThis();
	}

	private void DespawnThis()
	{
		if (trailRenderer != null)
		{
			trailRenderer.ShotDone();
		}
		rigidBody.velocity = Vector2.zero;
		Despawn.ClientSingle(gameObject);
	}
}