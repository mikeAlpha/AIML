using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    NavMeshAgent mAgent;
    public Transform Target;

    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAgent.SetDestination(Target.position);
    }
}
