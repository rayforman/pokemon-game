using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyMotion : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject captureRangeScript;
    public GameObject player;
    public Material projMaterial;
    public GameObject target;
    public float direction = 1.0f;         
    public float speed = 20.0f;
    public const float STRENGTH = 5.0f;
    private bool inRange = false;
    private bool isAggressive = false;
    private bool inHand = true;
    private Quaternion lastRotation;
    private Vector3 ogPos;      // stores starting position
    private float curAngle = 0.0f;
    public GameObject projPrefab;
    private GameObject projectile;
     private Rigidbody rbProj;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initiatlize projectile materials to transparent
        projMaterial.color = new Color(projMaterial.color.r, projMaterial.color.g, projMaterial.color.b, 0.0f);
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update inRange from CaptureRange Script
        inRange = captureRangeScript.GetComponent<CaptureRange>().inRange;

        // Not in capture range, normal movement
        if (!inRange)
            RegularMovement();

        // In capture range, aggressive state activated
        else
        {
            // AGGRESSIVE STATE
            if (!isAggressive)      // first time call
            {
                lastRotation = transform.rotation;
                isAggressive = true;

                // projectiles appear
                projMaterial.color = new Color(projMaterial.color.r, projMaterial.color.g, projMaterial.color.b, 1.0f);
            }
            AggressiveState();
        }
    }

    void RegularMovement()
    {
        if (isAggressive)           // if coming from aggressive state
        {
            transform.rotation = lastRotation;       // restore last orientation
            isAggressive = false;   // disable flag

            // hide projectiles
            projMaterial.color = new Color(projMaterial.color.r, projMaterial.color.g, projMaterial.color.b, 0.0f);
        }
        
        // Spin the object around the target
        transform.RotateAround(target.transform.position, Vector3.up, direction * speed * Time.deltaTime);
        // Update angle
        curAngle += Mathf.Abs(direction * speed * Time.deltaTime);
    }

    void AggressiveState()
    {
        // Look at Player
        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0.0f, dir.z);
        transform.LookAt(transform.position - dir, Vector3.up);     // look at player, ignore y component
        // Debug.Log(transform.forward);

        // Projectiles begin throwing, only if projectile is on hand
        if (inHand)
            FlyThrowProj();
    }

    void FlyThrowProj()
    {
        // Spawn object to throw
        Vector3 spawnPoint = transform.position + transform.forward*-1 + Vector3.up;
        projectile = Instantiate(projPrefab, spawnPoint, Quaternion.identity);
        Rigidbody projRB = projectile.GetComponent<Rigidbody>();

        // Throw object
        Vector3 throwDirection = player.transform.position-projRB.position;
        Vector3 impulse = (throwDirection) * 1.7f;
        projRB.AddForce(impulse, ForceMode.Impulse);
        projRB.useGravity = true;
        inHand = false;

        Invoke("FlyReturnProj", 3.0f);
    }

    void FlyReturnProj()
    {
        Destroy(projectile);
        inHand = true;
    }
}
