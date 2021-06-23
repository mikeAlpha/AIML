using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mikealpha.AI.FSM;

[CreateAssetMenu(menuName = "Arte De MikeAlpha/AI/Actions/Patrol")]
public class Patrol : Action
{
    public override void Act(StateController controller)
    {
        DoPatrol(controller);
    }

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    private void DoPatrol(StateController controller)
    {
        //controller.anim.SetFloat("Speed", 1.0f);
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            controller.navMeshAgent.isStopped = false;
            controller.navMeshAgent.destination = controller.wayPointList[controller.nextWaypoint].transform.position;
            controller.nextWaypoint = (controller.nextWaypoint + 1) % controller.wayPointList.Count;
        }
    }

}
