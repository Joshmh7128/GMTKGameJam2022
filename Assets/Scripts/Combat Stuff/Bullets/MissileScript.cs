using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : BulletScript
{
	protected override void Accelerate()
	{
		velocity = velocity.normalized * Mathf.Lerp(initialSpeed, acceleration * maxLifeTime, Mathf.Pow(time / maxLifeTime, 2));
	}
}
