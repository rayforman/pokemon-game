using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtCreature : MonoBehaviour
{
    public GameObject orb;
    public GameObject creature;
    public GameObject captureRangeScript;
    public GameObject winLoseScript;
    public Material prMaterial;
    private Collider orbCollider;
    private Collider creatureCollider;
    private bool orbTouching = false;
    private bool inRange = false;
    private bool isCaught = false;

    
    // Start is called before the first frame update
    void Start()
    {
        // Part room material is transparent until caught
        prMaterial.color = new Color(prMaterial.color.r, prMaterial.color.g, prMaterial.color.b, 0.0f);
        orbCollider = orb.GetComponent<Collider>();     // orb's collider
        creatureCollider = creature.GetComponent<Collider>();    // creature's collider
    }

    // Update is called once per frame
    void Update()
    {
        // Update inRange, orbTouching
        inRange = captureRangeScript.GetComponent<CaptureRange>().inRange;
        orbTouching = orbCollider.bounds.Intersects(creatureCollider.bounds);
        
        if (inRange && orbTouching)
        {
            Catch();
        }
    }

    void Catch()
    {
        isCaught = true;
        creature.SetActive(false);   // make creature disappear
        Debug.Log("Caught: " + creature.name);
        
        // ADD TO PARTY ROOM
        prMaterial.color = new Color(prMaterial.color.r, prMaterial.color.g, prMaterial.color.b, 1.0f);

        //Update win-lose script booleans
        if (creature.name == "bug_1" || creature.name == "bug_2" || creature.name == "bug_3")
            winLoseScript.GetComponent<WinLoseChecker>().caughtBug = true;
        else if (creature.name == "fly_1" || creature.name == "fly_2" || creature.name == "fly_3")
            winLoseScript.GetComponent<WinLoseChecker>().caughtFly = true;
        else if (creature.name == "fish_1" || creature.name == "fish_2" || creature.name == "fish_3")
            winLoseScript.GetComponent<WinLoseChecker>().caughtFish = true;
    }
}
