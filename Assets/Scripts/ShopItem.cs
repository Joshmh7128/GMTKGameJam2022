using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // our face
    public FaceClass face;
    [SerializeField] bool canInteract;

    private void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if (canInteract)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                DiceClass.instance.SwapWeapon(0, face);
                Destroy(gameObject);
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                DiceClass.instance.SwapWeapon(1, face);
                Destroy(gameObject);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                DiceClass.instance.SwapWeapon(2, face);
                Destroy(gameObject);
            }

            if (Input.GetKey(KeyCode.Alpha4))
            {
                DiceClass.instance.SwapWeapon(3, face);
                Destroy(gameObject);
            }

            if (Input.GetKey(KeyCode.Alpha5))
            {
                DiceClass.instance.SwapWeapon(4, face);
                Destroy(gameObject);
            }

            if (Input.GetKey(KeyCode.Alpha6))
            {
                DiceClass.instance.SwapWeapon(5, face);
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        canInteract = false;
    }
}
