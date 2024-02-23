using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbThrow : MonoBehaviour
{
    public Rigidbody orb;
    public Slider slider;
    public Button button;
    public Camera camera;
    public float strength = 10.0f;
    private float sliderValue;
    private Vector3 cameraDirection;
    private Vector3 orbStart;
    private bool inHand = true;
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        orbStart = orb.transform.position;
    }

    void Update()
    {
        // Debug.Log("Updating");
        cameraDirection = camera.transform.forward;

        // Return Ball to Hand
        if (inHand)
            orb.transform.position = camera.transform.position + cameraDirection*0.75f + Vector3.up*-0.51f;
    }

    void OnButtonClick()
    {
        // Debug.Log("Button Clicked!");
        if (inHand)
        {
            // Update Values
            cameraDirection = camera.transform.forward;
            sliderValue = slider.value/100f - 0.5f;
            ThrowOrb();
        }
    }

    void ThrowOrb()
    {
        inHand = false;
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraDirection);
        cameraRight = cameraRight.normalized;

        // Adds up impulse proportional to vertical camera viewing angle
        // Debug.Log(Vector3.Dot(cameraDirection, Vector3.up));
        float upScalar = Vector3.Dot(cameraDirection, Vector3.up) + 0.3f;

        // Add impulse for throw
        Vector3 impulseDirection = cameraDirection + cameraRight*sliderValue + Vector3.up*upScalar;
        Vector3 impulse = impulseDirection * strength;
        orb.AddForce(impulse, ForceMode.Impulse);
        orb.useGravity = true;
        
        // Return ball to hand after specified time
        Invoke("ReturnBall", 2.0f);
    }

    void ReturnBall()
    {
        orb.velocity = Vector3.zero;
        orb.useGravity = false;
        inHand = true;
    }
}
