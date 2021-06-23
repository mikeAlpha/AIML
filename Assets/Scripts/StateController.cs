using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mikealpha.AI.FSM;

public class StateController : MonoBehaviour
{
    public Transform eyePosition;
    public State RemainInState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public int nextWaypoint = 0;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public Vector3 LastKnownPosition;

    public Transform[] CoverPoints;

    public List<Transform> wayPointList;
    public State currentState;

    private bool aiActive;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        SetupAI(true);
    }

    public void SetupAI(bool aiActivation)
    {
        aiActive = aiActivation;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != RemainInState)
        {
            currentState = nextState;
            OnExitTime();
        }
    }

    void OnExitTime()
    {

    }
}
