using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
    public NavMeshAgent agent;
    public Transform hpBarCanvas;
    public bool active;

    private Transform target;
    private PlayerBehaviour player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupHealth();
    }

    public void GetTarget()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        target = FindObjectOfType<OrbitCamera>().transform;
    }
    
    void Update()
    {
        if (target != null)
        {
            if (active == false)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 5)
                {
                    active = true;
                }
            }
            if (active)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > 20)
                {
                    active = false;
                }
                if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
                {
                    agent.SetDestination(transform.position);
                }
                else
                {
                    FoundPlayer();
                }
            }
            hpBarCanvas.LookAt(target);
        }
    }

    void FoundPlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}
