using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
    public NavMeshAgent agent;
    public Transform hpBarCanvas;
    public bool active;

    private Animator anim;
    private Transform target;
    private PlayerBehaviour player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("Idle", true);
        SetupHealth();
    }

    public void GetTarget()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        target = FindObjectOfType<OrbitCamera>().transform;
    }
    
    void Update()
    {
        if (dead == false)
        {
            if (target != null)
            {
                if (active == false)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < 10)
                    {
                        active = true;
                        anim.SetBool("Idle", false);
                    }
                }
                if (active)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) > 20)
                    {
                        active = false;
                        anim.SetBool("Idle", true);
                    }
                    if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
                    {
                        anim.SetBool("Walking", false);
                        agent.SetDestination(transform.position);
                    }
                    else
                    {
                        anim.SetBool("Walking", true);
                        FoundPlayer();
                    }
                }
                hpBarCanvas.LookAt(target);
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (dead == false)
        {
            anim.SetTrigger("GetHit");
        }
        base.TakeDamage(damage);
    }

    public override void OnDeath()
    {
        agent.enabled = false;
        GetComponent<CharacterController>().enabled = false;
        hpBarCanvas.gameObject.SetActive(false);
        anim.SetTrigger("Death");
    }

    void FoundPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    public void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
        {
            player.TakeDamage(25);
        }
    }
}
