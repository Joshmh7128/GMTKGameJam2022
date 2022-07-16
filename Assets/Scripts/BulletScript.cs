using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	[Tooltip("Life time of the projectile."), SerializeField]
	float maxLifeTime = 5f;

	[Tooltip("Layers this projectile can collide with."), SerializeField]
	LayerMask hittableLayers = -1;

	[Header("Movement"), Tooltip("Speed of the projectile."), SerializeField]
	float speed = 20f;

	[Tooltip("Deceleration rate of the bullet."), SerializeField]
	float slowdown = 0f;

	[Header("Damage"), Tooltip("Damage of the projectile."), SerializeField]
	float damage = 40f;

	[Header("FX"), Tooltip("FX for the object"), SerializeField]
	GameObject particleFX;
	[SerializeField] bool parentFX;

	Vector3 velocity;

	private void OnEnable()
	{
		Destroy(gameObject, maxLifeTime);
		velocity = transform.forward * speed;
		if (!parentFX)
		{ Instantiate(particleFX, transform.position, transform.rotation); } // need to rewrite as a pooled spawn visual effect 

		if (parentFX)
		{ Instantiate(particleFX, transform); } // need to rewrite as a pooled spawn visual effect 
	}

	private void Update()
	{
		transform.position += velocity * Time.deltaTime;
		velocity = velocity.normalized * (velocity.magnitude - slowdown * Time.deltaTime);
	}
}
