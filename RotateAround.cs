using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;
    public float direction = 1.0f;          // 1.0 for CW, -1.0 for CCW
    public float speed = 20.0f;

    void Update()
    {
        // Spin the object around the target
        transform.RotateAround(target.transform.position, Vector3.up, direction * speed * Time.deltaTime);
    }
}
