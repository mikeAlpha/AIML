using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mikealpha.AI.Pathfinding;

public class Unit : MonoBehaviour
{
    public float speed = 5;
    public Transform target;

    Vector3[] pathFound;
    Vector3 waypoint;
    bool IsMoving = false;

    public LayerMask layer;

    Rigidbody rBody;
    Animator anim;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        anim = GetComponent<Animator>();
        
        
        //rBody = GetComponent<Rigidbody>();
        //RagDollDeath(false);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    RagDollDeath(true);
        //}

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                Debug.Log(hitInfo.transform.name);

                if (!hitInfo.transform.GetComponent<Rigidbody>())
                    return;

                RagDollDeath(true);
                Debug.Log(hitInfo.transform.name);
                Rigidbody r = hitInfo.collider.GetComponent<Rigidbody>();
                r.AddForce(hitInfo.point);
            }
        }
    }

    void RagDollDeath(bool state)
    {
        Collider[] cBodies = GetComponentsInChildren<Collider>();
        foreach (Collider c in cBodies)
        {
            if (c == GetComponent<Collider>())
                continue;
            c.enabled = state;
        }
        anim.enabled = !state;
        //rBody.useGravity = !state;
        GetComponent<Collider>().enabled = !state;
    }

    public void Move()
    {
        IsMoving = true;
        anim.SetFloat("Speed", 1.0f);
    }

    public void Stop()
    {
        IsMoving = false;
        anim.SetFloat("Speed", 0.0f);
    }

    void OnPathFound(Vector3[] newPath, bool success)
    {
        pathFound = newPath;
        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        waypoint = pathFound[0];
        int index = 0;

        while (true)
        {
            if (waypoint == transform.position)
            {
                index++;
                if (index >= pathFound.Length)
                    yield break;

                waypoint = pathFound[index];
            }

            waypoint.y = transform.position.y;

            if (IsMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
                //rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y, speed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoint, 0.0f);
                //rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y, 0.0f);
            }

            //Vector3 LookPosition = waypoint * Time.deltaTime * 50;
            //transform.LookAt(LookPosition);

            transform.LookAt(waypoint);

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if (pathFound != null)
        {
            for (int i = 0; i < pathFound.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(pathFound[i], Vector3.one * 0.5f);

                if (i > 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(pathFound[i - 1], pathFound[i]);
                }
            }
        }
    }
}
