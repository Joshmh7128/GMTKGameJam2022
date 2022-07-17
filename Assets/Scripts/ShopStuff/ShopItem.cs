using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // our face
    public FaceClass face;
    [SerializeField] bool canInteract;
    [SerializeField] int cost; // how much does this item cost?
    [SerializeField] Text costDisplay; // show how much we cost

    private void Start()
    {
        if (costDisplay != null)
        costDisplay.text = "Cost: " + cost;
    }

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
                BuyWeapon(1, cost);
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {

                BuyWeapon(2, cost);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {

                BuyWeapon(3, cost);
            }

            if (Input.GetKey(KeyCode.Alpha4))
            {

                BuyWeapon(4, cost);
            }

            if (Input.GetKey(KeyCode.Alpha5))
            {

                BuyWeapon(5, cost);
            }

            if (Input.GetKey(KeyCode.Alpha6))
            {

                BuyWeapon(6, cost);
            }
        }
    }

    void BuyWeapon(int slot, int cost)
    {
        if (Dice.Player.PlayerCharacterController.currencyAmount > cost)
        {
            DiceClass.instance.SwapWeapon(slot-1, face);
            Dice.Player.PlayerCharacterController.currencyAmount -= cost;
            Destroy(gameObject);
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
