using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOnPointClick : MonoBehaviour
{

    public CharacterController cc;
    public Camera myCamera;
    Vector3 movePosition;
    bool IsMoving = false;

    Animator anim;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray myRay = myCamera.ScreenPointToRay(mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(myRay, out hitInfo, Mathf.Infinity))
            {
                movePosition = hitInfo.point;
                IsMoving = true;
            }
        }

        if (IsMoving)
        {
            Vector3 pos = new Vector3(movePosition.x, transform.position.y, movePosition.z);
            transform.LookAt(pos);
            cc.Move((pos - transform.position) * 2f * Time.deltaTime);

            //anim.SetFloat("MoveZ", 1f);
        }

    }
}
