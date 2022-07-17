using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	[Tooltip("Life time of the projectile."), SerializeField]
	protected float maxLifeTime = 5f;

	// Time the projectile has existed
	protected float time;

	[Tooltip("Layers this projectile can collide with."), SerializeField]
	protected LayerMask hittableLayers = -1;

	[Header("Movement"), Tooltip("Speed of the projectile."), SerializeField]
	protected float initialSpeed = 20f;

	[Tooltip("Acceleration rate of the bullet."), SerializeField]
	protected float acceleration = 0f;

	[Header("Damage"), Tooltip("Damage of the projectile."), SerializeField]
	protected float damage = 40f;

	protected Vector3 velocity;

	protected void OnEnable() {
		velocity = transform.forward * initialSpeed;
		time = 0;
	}

	protected void Update() {
		transform.position += velocity * Time.deltaTime;
		time += Time.deltaTime;
		Accelerate();

		// "Destroy" our object after its lifespan.
		if (time >= maxLifeTime)
			ObjectPooler.instance.RequeueObject(gameObject);
	}

	virtual protected void Accelerate() {
		velocity = velocity.normalized * (velocity.magnitude + acceleration * Time.deltaTime);
	}
}
