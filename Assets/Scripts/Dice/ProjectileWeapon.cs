using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponClass {

	public float coolDown;
	private float _coolDown;
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform bulletOrigin;
	public GameObject weaponModel; // the model of our weapon to be instantiated on other places

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

	private void Update() {
		if (_coolDown > 0) {
			_coolDown -= Time.deltaTime;
		}
	}
}
