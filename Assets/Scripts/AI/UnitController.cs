using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{

    public Unit mUnit;
    float HoldTimer = 0.0f;
    bool pointerDown = false;

    void Update()
    {
        if (pointerDown)
        {
            HoldTimer += Time.deltaTime;
            if (HoldTimer > 0.1f)
            {
                //Debug.Log(HoldTimer);
                mUnit.Move();
            }
        }
    }   

    public void OnPressed()
    {
        pointerDown = true;
    }

    public void OnRelease()
    {
        pointerDown = false;
        HoldTimer = 0.0f;
        mUnit.Stop();
    }
}