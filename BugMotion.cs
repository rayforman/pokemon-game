using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BugMotion : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject captureRangeScript;
    public GameObject player;
    public Material projMaterial;
    public float speed = 3.0f;
    public float distance = 3.0f;
    private float facing = -1.0f;
    private const float rotationSpeed = 15.0f;
    private bool inRange = false;
    private bool isAggressive = false;
    private bool inHand = true;
    private Vector3 startPosition;
    private Vector3 startDirection;
    private Quaternion lastRotation;
    public const float STRENGTH = 5.0f;
    public GameObject projPrefab;
    private GameObject projectile;
    private Rigidbody rbProj;

    void Start()
    {
        // Initiatlize projectile materials to transparent
        projMaterial.color = new Color(projMaterial.color.r, projMaterial.color.g, projMaterial.color.b, 0.0f);
        startPosition = transform.position;
        startDirection = transform.forward;
    }

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
        // Debug.Log(transform.position);
        // Flip flop direction
        if (Vector3.Dot((transform.position - startPosition), startDirection) >= distance || 
            Vector3.Dot((transform.position - startPosition), startDirection) < 0.0f)     // time to turn
        {
            transform.Rotate(Vector3.up, 180.0f);
            // Debug.Log("Turn around, walk other way");
        }
        
        // Move
        transform.Translate(Vector3.forward * speed * facing * Time.deltaTime);
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
            BugThrowProj();
    }

    void BugThrowProj()
    {
        // Spawn object to throw
        Vector3 spawnPoint = transform.position + transform.forward*-1 + Vector3.up;
        projectile = Instantiate(projPrefab, spawnPoint, Quaternion.identity);
        Rigidbody projRB = projectile.GetComponent<Rigidbody>();

        // Throw object
        Vector3 throwDirection = player.transform.position-projRB.position;
        float upScalar = Vector3.Dot(throwDirection, Vector3.up) + 0.2f;
        Vector3 impulse = (throwDirection + Vector3.up*upScalar) * 2.0f;
        projRB.AddForce(impulse, ForceMode.Impulse);
        projRB.useGravity = true;
        inHand = false;

        Invoke("BugReturnProj", 3.0f);
    }

    void BugReturnProj()
    {
        Destroy(projectile);
        inHand = true;
    }
}
