using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClass : MonoBehaviour
{
	public GameObject weapon;
    GameObject displayWeapon;
    public int diePosition; // 1 through 6

    // on enable spawn our weapon to the face spot
    public void UpdateDisplay(int position)
    {
        Destroy(displayWeapon);
        displayWeapon = Instantiate(weapon.GetComponent<WeaponClass>().weaponModel, DiceClass.instance.faceDisplayTransforms[position]);
    }
}
