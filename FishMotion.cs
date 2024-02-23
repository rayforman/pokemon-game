using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMotion : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject captureRangeScript;
    public GameObject player;
    public GameObject range;
    public GameObject orb1, orb2, orb3, orb4;
    // public Rigidbody projectile;
    public Material projMaterial;
    public GameObject target;
    private bool inRange = false;
    private bool isAggressive = false;
    private bool inHand = true;
    public Rigidbody rb;
    public float direction = 1.0f;       
    public float speed = 20.0f;
    private Quaternion lastRotation;
    private Vector3 lastVelocity;
    private Vector3 ogPos;      // stores starting position
    private Quaternion ogRot;      // stores starting rotation orientation
    private Vector3 curPos;     // stores current position
    private float curAngle = 0.0f;
    private bool isSwimming = true;
    private const float g = 9.8f;   // gravitation acceleration approximated near earth's surface
    private const float amplitude = 0.05f;
    private const float frequency = 2.0f;
    public GameObject projPrefab;
    private GameObject projectile;
    private Rigidbody rbProj;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initiatlize projectile materials to transparent
        projMaterial.color = new Color(projMaterial.color.r, projMaterial.color.g, projMaterial.color.b, 0.0f);
        ogPos = transform.position;
        ogRot = transform.rotation;
        // Debug.Log(ogPos.y);
        rb.useGravity = false;
        isSwimming = true;
        rb.AddForce(new Vector3(0,-10,0), ForceMode.Impulse);     // initial bobbing impulse
    }

    // Update is called once per frame
    void Update()
    {
        // Keep projectile in center as long as in hand
        // if (inHand)
        //     projectile.transform.position = transform.position;
        
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
                lastVelocity = rb.velocity;
                rb.velocity = Vector3.zero;  // stops all motion
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
            rb.velocity = lastVelocity;              // restore last velocity
            isAggressive = false;   // disable flag

            // hide projectiles
            projMaterial.color = new Color(projMaterial.color.r, projMaterial.color.g, projMaterial.color.b, 0.0f);
        }
        
        if (!isSwimming && rb.position.y < ogPos.y && rb.velocity.y < 0)        // just landed: stop using gravity
        {
            range.SetActive(true);          // make range active again
            orb1.SetActive(true);           // make orbs active again
            orb2.SetActive(true);
            orb3.SetActive(true);
            orb4.SetActive(true);
            
            isSwimming = true;
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, ogPos.y, transform.position.z);
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            transform.rotation = ogRot;                 // FIX ORIENTATION
            rb.angularVelocity = Vector3.zero;      // undo spin
            rb.AddForce(new Vector3(0,-10,0), ForceMode.Impulse);        //initial impulse again
            // Debug.Log("Landed");
        }
        else if (curAngle >= 360.0f)         // if made full revolution: Jump
        {
            range.SetActive(false);          // make range inactive during jump
            orb1.SetActive(false);           // make orbs inactive during jump
            orb2.SetActive(false);
            orb3.SetActive(false);
            orb4.SetActive(false);
            
            // add jump impulse, and apply gravity
            isSwimming = false;
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(new Vector3(0,45,0), ForceMode.Impulse);
            rb.AddTorque(Vector3.right * -5 * direction, ForceMode.Impulse);
            rb.useGravity = true;
            // reset angle
            curAngle = 0.0f;

            // rb.useGravity = true;
            // Debug.Log("Jumping");
        }
        else if (isSwimming)         // in water: apply bobbing
        {
            // Spin the object around the target
            transform.RotateAround(target.transform.position, Vector3.up, direction * speed * Time.deltaTime);

            // Update angle
            curAngle += Mathf.Abs(direction * speed * Time.deltaTime);
            
            // //Debug.Log("Bobbing");
            if (isSwimming && transform.position.y > ogPos.y)
            {   
                rb.AddForce(new Vector3(0,-1,0), ForceMode.Impulse);
                // Debug.Log("above");
            }
            else if (isSwimming && transform.position.y < ogPos.y)
            {
                rb.AddForce(new Vector3(0,1,0), ForceMode.Impulse);
                // Debug.Log("below");
            }
            // Debug.Log("Bobbing");
        }
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
            FishThrowProj();
    }

    void FishThrowProj()
    {
        // Spawn object to throw
        Vector3 spawnPoint = transform.position + transform.forward*-1 + Vector3.up;
        projectile = Instantiate(projPrefab, spawnPoint, Quaternion.identity);
        Rigidbody projRB = projectile.GetComponent<Rigidbody>();

        // Throw object
        Vector3 throwDirection = player.transform.position-projRB.position;
        float upScalar = Vector3.Dot(throwDirection, Vector3.up) + 1.5f;
        Vector3 impulse = (throwDirection + Vector3.up*upScalar) * 2.0f;
        projRB.AddForce(impulse, ForceMode.Impulse);
        projRB.useGravity = true;
        inHand = false;

        Invoke("FishReturnProj", 3.0f);
    }

    void FishReturnProj()
    {
        Destroy(projectile);
        inHand = true;
    }
}
