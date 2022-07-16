using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dice.Player;

public class ZombieEnemy : EnemyClass
{
    // 

    [SerializeField] GameObject ragDoll;
    float knockBackForce;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1, true);
        }
    }

    public override void TakeDamage(int damage)
    {        
        // take damage
        HP -= damage;
        // check if we have died, and if we have, run the death function
        if (HP <= 0)
        {
            OnDeath();
        }
    }

    public override void TakeDamage(int damage, bool knockBack)
    {
        // take damage
        HP -= damage;
        // set knockBack
        knockBackForce = 10f;
        // check if we have died, and if we have, run the death function
        if (HP <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        // instantiate ragdoll
        Rigidbody ragDollBody = Instantiate(ragDoll, transform.position, transform.rotation).GetComponent<Rigidbody>();
        // apply force to the ragdoll body based on the damage
        Vector3 heading = gameObject.transform.position - FindObjectOfType<PlayerCharacterController>().transform.position ;
        ragDollBody.AddForce(heading * knockBackForce, ForceMode.Impulse);
        // disable body
        gameObject.SetActive(false);

    }
}
