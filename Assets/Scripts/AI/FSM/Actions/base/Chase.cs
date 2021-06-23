using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mikealpha.AI.FSM;

[CreateAssetMenu(menuName = "Arte De MikeAlpha/AI/Actions/Chase")]
public class Chase : Action
{
    public override void Act(StateController controller)
    {
        ChaseAction(controller);
    }

    public override void Draw()
    {
        
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    void ChaseAction(StateController controller)
    {

        Debug.DrawRay(controller.eyePosition.position, controller.eyePosition.forward * 40f, Color.red);
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }
}
