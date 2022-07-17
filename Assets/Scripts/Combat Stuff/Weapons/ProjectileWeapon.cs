using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponClass {

	public float coolDown;
	protected float _coolDown;
	[SerializeField] protected GameObject projectilePrefab;
	[SerializeField] protected Transform bulletOrigin;

    public override void Attack() {
		if (_coolDown <= 0 && uses > 0) {
			// Fire projectile from player.
			ObjectPooler.instance.SpawnFromPool(projectilePrefab, bulletOrigin.position, bulletOrigin.rotation);

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

	protected void Update() {
		if (_coolDown > 0) {
			_coolDown -= Time.deltaTime;
		}
	}
}
