using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AItest : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] targets;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            agent.SetDestination(targets[Random.Range(0, targets.Length)].position);
            anim.SetFloat("Move", 1f);
        } 
    }
}
 