using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float smoothing;

    private Vector3 offset;
  
    void Start()
    {
        offset = transform.position - Target.position;
    }
   
    void FixedUpdate()
    {
        Vector3 targetPos = Target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
