using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClass : MonoBehaviour
{
	public GameObject weapon;
    [SerializeField]GameObject displayWeapon;
    public int diePosition; // 1 through 6

    // on enable spawn our weapon to the face spot
    public void UpdateDisplay(int position)
    {
        displayWeapon = Instantiate(weapon.GetComponent<WeaponClass>().weaponModel, DiceClass.instance.faceDisplayTransforms[position]);
    }

    public void Clean()
    {
        Destroy(displayWeapon);
    }
}
