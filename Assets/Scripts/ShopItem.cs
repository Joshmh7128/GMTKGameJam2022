using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // our face
    public FaceClass face;

    private void Update()
    {


    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            DiceClass.instance.SwapWeapon(1, face);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            DiceClass.instance.SwapWeapon(2, face);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            DiceClass.instance.SwapWeapon(3, face);
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            DiceClass.instance.SwapWeapon(4, face);
        }

        if (Input.GetKey(KeyCode.Alpha5))
        {
            DiceClass.instance.SwapWeapon(5, face);
        }

        if (Input.GetKey(KeyCode.Alpha6))
        {
            DiceClass.instance.SwapWeapon(6, face);
        }

    }

}
