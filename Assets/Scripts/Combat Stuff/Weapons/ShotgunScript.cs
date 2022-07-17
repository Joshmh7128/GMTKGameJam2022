using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : ProjectileWeapon
{
	[SerializeField] float rotationThing = 7f;

	public override void Attack()
	{
		if (_coolDown <= 0 && uses > 0)
		{
			// Fire projectile from player.
			for (int i = 0; i < 3; i++)
			{
				float num = Random.Range(-rotationThing, rotationThing);
				bulletOrigin.Rotate(new Vector3(0, num, 0));
				GameObject obj = ObjectPooler.instance.SpawnFromPool(projectilePrefab, bulletOrigin.position, bulletOrigin.rotation);
				bulletOrigin.Rotate(new Vector3(0, -num, 0));
			}

			// Play shooting animation from weapon.
			// Do that.

			uses -= 1;
			_coolDown = coolDown;
		}

		if (_coolDown <= 0 && uses <= 0)
		{
			DiceClass.instance.RollTheDie();
		}
	}
}
