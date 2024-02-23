using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToDance : MonoBehaviour
{
    public Camera cam;
    public Rigidbody creature;
    private Touch touch;
    private Vector3 ogPos;
    private bool isDancing = false;
    

    // Start is called before the first frame update
    void Start()
    {
        ogPos = creature.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.enabled && Input.touchCount > 0)
        {
            for (int i=0; i<Input.touchCount; i++)  // get all current touches
            {
                touch = Input.GetTouch(i);  // get touch on screen

                if (touch.phase == TouchPhase.Began)    // if its the beginning of the touch
                {
                    Ray ray = cam.ScreenPointToRay(touch.position);   // send ray through touched point
                    RaycastHit hit;                                   // declare output variable

                    if (Physics.Raycast(ray, out hit))                // only executes on successful hits
                    {
                        // Debug.Log(hit.collider.GetComponent<Rigidbody>());
                        if (hit.collider.GetComponent<Rigidbody>() == creature)
                        {
                            isDancing = !isDancing;
                            // Debug.Log("Touched!");
                            // Debug.Log(isDancing);
                        }
                    }
                }
            }
        }

        if (isDancing)
            Dance(creature);
    }

    void Dance(Rigidbody rb)
    {
        // Debug.Log("In dancing function");
        if (rb.position.y <= ogPos.y)
        {
            // Jump and Spin dance
            rb.AddForce(new Vector3(0,3,0), ForceMode.Impulse);
            rb.AddTorque(Vector3.up*5, ForceMode.Impulse);
        }
    }
}
