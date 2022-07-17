using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClass : MonoBehaviour
{
	public GameObject weapon;
    //public WeaponClass weaponClass;

    // on enable spawn our weapon to the face spot
    private void OnEnable()
    {
        Instantiate(weapon.GetComponent<WeaponClass>().weaponModel, transform);
    }
}
