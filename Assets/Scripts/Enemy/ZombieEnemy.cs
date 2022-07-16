using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : EnemyClass
{
    // 

    [SerializeField] GameObject ragDoll;
    [SerializeField] float knockBackForce;

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

        // disable body
        gameObject.SetActive(false);

    }
}
