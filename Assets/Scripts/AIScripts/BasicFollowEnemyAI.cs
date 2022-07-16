using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicFollowEnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    Vector3 wanderTarget = Vector3.zero;
    Vector3 lastSeen;
    public float visDist = 10;
    public float capDist = 2;
    public bool cooledDown = true;
    [SerializeField] bool usesSight; // added by josh to control enemy behaviour

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Pursue()
    {
        //get the direction of target
        Vector3 targetDir = target.transform.position - transform.position;

        //get angle guard should approach
        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

		{//gets targets speed to predict where they are going
		 /* if (toTarget > 90 && relativeHeading < 20 || target.GetComponent<PlayerController>().moveSpeed < 0.01f)
		  {
			  //approch target, uses speed to determine where target is going
			  Seek(target.transform.position);
			  return;
		  }*/

			//go to object with objects speed in mind
			// float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<PlayerController>().moveSpeed);
			// Seek(target.transform.position + target.transform.forward * lookAhead);
		}

		Seek(target.transform.position);
    }

    void Wander()
    {
        //distance for waypoint, as well as randomness
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = .5f;

        //makes waypoint random
        wanderTarget += new Vector3(Random.Range(-5.0f, 5.0f) * wanderJitter, 0, Random.Range(-5.0f, 5.0f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        //calculates position for next target area
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.TransformVector(targetLocal);

        //goes to random area
        Seek(targetWorld);
    }

    bool CanSeeTarget()
    {
        //is there a line of sight between the target and guard, if so, can see
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
                return true;
        }
        if (usesSight)
        {
            return false;
        } 

        return true;
    }

    bool TargetInRange()
    {
        //is target in seeing distance
        if (Vector3.Distance(transform.position, target.transform.position) < visDist)
            return true;
        return false;
    }

    public void Attack()
    {
		{/* anim.SetBool("IsAttack", true);

        yield return new WaitForSeconds(.5f);

        anim.SetFloat("Speed", 0);
        attackBox.SetActive(true);
        yield return new WaitForSeconds(.5f);
        attackBox.SetActive(false);
        anim.SetBool("IsAttack", false);
        anim.SetFloat("Speed", 1);
        yield return new WaitForSeconds(5);
        cooledDown = true;*/}
    }

    // Update is called once per frame
    void Update()
    {
        if (!TargetInRange())
        {
            //wander if cant see target
            Wander();
        }
        else if (TargetInRange() && CanSeeTarget())
        {
            // Pursue if can see target, make tab on where they were last seen
            Pursue();
        }

        //if target is in rage, they are captured
        Vector3 direction = target.transform.position - transform.position;

        if (direction.magnitude < capDist && cooledDown)
        {
            Attack();
            cooledDown = false;
        }
    }
}
