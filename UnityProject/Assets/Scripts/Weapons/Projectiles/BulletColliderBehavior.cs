using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons.Projectiles.Behaviours;

/// <summary>
/// Handles the collision logic for BulletBehavior, responding to the rigidbody collision events and passing them up
///  to BulletBehavior.
///
/// Has to be separate from BulletBehavior because BulletBehavior exists on the parent gameobject, so is unable
/// to respond to collision events on this gameobject.
/// </summary>
public class BulletColliderBehavior : MonoBehaviour
{
	/// <summary>
	/// Cached bulletbehavior in the parent
	/// </summary>
	private List<IBulletBehaviour> bulletBehaviours;

	private void Awake()
	{
		bulletBehaviours = GetComponentsInParent<IBulletBehaviour>().ToList();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		foreach (var behaviour in bulletBehaviours)
		{
			behaviour.HandleCollisionEnter2D(other);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		foreach (var behaviour in bulletBehaviours)
		{
			behaviour.HandleTriggerEnter2D(other);
		}
	}
}
