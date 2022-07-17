using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dice.Player;


public class HurtWall : MonoBehaviour
{
    public float speed;
    public Transform target;

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distancePerFrame, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerCharacterController.instance.TakeDamage(1000);
        }
    }
}
