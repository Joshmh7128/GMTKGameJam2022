using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHit : MonoBehaviour
{
    public GameObject dollarParticles;

    public void Start(){
        dollarParticles.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HitMoney");
        dollarParticles.SetActive(true);

    }
}
