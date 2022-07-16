using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : EnemyAI
{
    //[Header("Close Range")]
    //public GameObject attackBox;
    public new virtual IEnumerator Attack()
    {
            // anim.SetBool("IsAttack", true);
            agent.speed = 0;
            yield return new WaitForSeconds(.5f);

           // anim.SetFloat("Speed", 0);
            bulletPrefab.SetActive(true);
        // yield return new WaitForSeconds(.5f);
            bulletPrefab.SetActive(false);
            //anim.SetBool("IsAttack", false);
            // anim.SetFloat("Speed", 1);
            agent.speed = moveSpeed;
            yield return new WaitForSeconds(5);
            cooledDown = true;

    }
}
