using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureRange : MonoBehaviour
{
    public GameObject player;
    public GameObject range;
    public GameObject creature;
    public Material rangeMaterial;
    public bool inRange;    // accessed from elsewhere
    private float alpha = 0.5f;
    private Collider playerCollider;
    private Collider rangeCollider;

    
    // Start is called before the first frame update
    void Start()
    {
        // Restore material
        rangeMaterial.color = new Color(rangeMaterial.color.r, rangeMaterial.color.g, rangeMaterial.color.b, alpha);
        playerCollider = player.GetComponent<Collider>();
        rangeCollider = range.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        inRange = playerCollider.bounds.Intersects(rangeCollider.bounds);
        // Debug.Log(inRange);

        if (inRange)
        {
            // range.SetActive(false);
            rangeMaterial.color = new Color(rangeMaterial.color.r, rangeMaterial.color.g, rangeMaterial.color.b, 0.0f);
        }
        else
        {
            // range.SetActive(true);
            rangeMaterial.color = new Color(rangeMaterial.color.r, rangeMaterial.color.g, rangeMaterial.color.b, alpha);
        }
    }
}
