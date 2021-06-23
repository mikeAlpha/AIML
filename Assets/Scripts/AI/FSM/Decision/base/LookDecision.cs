using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mikealpha.AI.FSM;

[CreateAssetMenu(menuName = "Arte De MikeAlpha/AI/Decisions/Look")]
public class LookDecision : Decision
{

    public float fov = 180.0f;
    public Vector3 LastKnownPosition;
    public bool PlayerLost = false;

    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    bool Look(StateController controller)
    {
        Debug.DrawRay(controller.eyePosition.position, controller.eyePosition.forward * 40f, Color.green);
       
        RaycastHit hit;
        if (Physics.Raycast(controller.eyePosition.position, controller.eyePosition.forward * 40f, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                controller.chaseTarget = hit.transform;
                //PlayerLost = false;
                return true;
            }
        }
        return false;
    }
}
