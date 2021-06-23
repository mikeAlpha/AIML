using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mikealpha.AI.FSM;

[CreateAssetMenu(menuName = "Arte De MikeAlpha/AI/Actions/Cover")]
public class TakeCover : Action
{

    public Vector3 CoverOffset;

    public override void Act(StateController controller)
    {
        Cover(controller);
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

    void Cover(StateController controller)
    {
        for(int i = 0; i<controller.CoverPoints.Length; i++)
        {
            float dist = (controller.transform.position - controller.CoverPoints[i].transform.position).sqrMagnitude;
            if(dist < 0.5f)
            {
                controller.navMeshAgent.destination = controller.CoverPoints[i].transform.position + CoverOffset;
            }
        }
    }
}
