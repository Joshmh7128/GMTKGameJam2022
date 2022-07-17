using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dice.Player;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;

    public int damage = 1;

    //public Rigidbody rb;

    public Vector3 dir;
    //public GameObject impactEffect;

    public float explosionRadius = 0f;

    public float lifetime;

    private void Start()
    {
        dir = target.position - transform.position;

    }
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }


        float distancePerFrame = speed * Time.deltaTime;


        //if distance reaches target that frame instead of oncollisionEnter
        if (dir.magnitude <= distancePerFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distancePerFrame, Space.World);
        transform.LookAt(Vector3.forward);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
            if(explosionRadius > 0f)
            {
                Explode();
            }
        } 

    }




    void HitTarget()
    {
        /*GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);*/ //Add Explosion particles here
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage();
        }



        Destroy(gameObject);
    }

    void Explode()
    {
        //spawn Sphere and effect anything that goes in that radius
        Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in col)
        {
            if (collider.tag == "Player")
            {
                Damage();
            }
        }
    }

    void Damage()
    {
        PlayerCharacterController.instance.TakeDamage(damage);
       /* TestMove p = player.GetComponent<TestMove>();

        if (p != null)
        {
            p.TakeDamage(damage);
        }*/

    }




}
