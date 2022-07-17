using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sudoku : EnemyAI
{
    public new virtual IEnumerator Attack()
    {
        // anim.SetBool("IsAttack", true);
        agent.speed = 0;
        yield return new WaitForSeconds(.5f);

        // anim.SetFloat("Speed", 0);
        // yield return new WaitForSeconds(.5f);
        //anim.SetBool("IsAttack", false);
        // anim.SetFloat("Speed", 1);
        agent.speed = moveSpeed;
        cooledDown = true;
        Destroy(this.gameObject);
    }
}
