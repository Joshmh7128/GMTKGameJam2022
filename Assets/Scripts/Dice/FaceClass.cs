using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClass : MonoBehaviour
{
	public GameObject weapon;
    GameObject displayWeapon;
    //public WeaponClass weaponClass;

    // on enable spawn our weapon to the face spot
    private void UpdateDisplay()
    {
        displayWeapon = Instantiate(weapon.GetComponent<WeaponClass>().weaponModel, transform);
    }
}
