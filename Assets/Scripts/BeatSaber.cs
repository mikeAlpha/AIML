using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSaber : MonoBehaviour
{

    public GameObject[] cubes;
    public Transform[] points;
    float beat = 60 / 105;
    float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > beat)
        {
            GameObject cube = Instantiate(cubes[Random.Range(0, 2)], points[Random.Range(0, 4)]);
            cube.transform.localPosition = Vector3.zero;
            cube.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));
            timer -= beat;
        }
    }
}