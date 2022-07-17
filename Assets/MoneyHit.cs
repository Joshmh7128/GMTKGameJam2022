using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHit : MonoBehaviour
{
    public GameObject dollarParticles;

    public void Start()
    {
        dollarParticles.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HitMoney");
            dollarParticles.SetActive(true);
            dollarParticles.transform.parent = null;
            Destroy(gameObject);
            Dice.Player.PlayerCharacterController.instance.currencyAmount += 1;
        }
    }
}
