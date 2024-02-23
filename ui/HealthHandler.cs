using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public GameObject winLoseScript;
    public Material projMaterial;
    public GameObject[] hearts;
    private int numHearts;
    
    // Start is called before the first frame update
    void Start()
    {
        numHearts = hearts.Length;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (numHearts <= 0)
            winLoseScript.GetComponent<WinLoseChecker>().LoseGame();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ProjectileCollider"))
        {
            // Destroy projectile
            Destroy(collision.gameObject);
            hearts[--numHearts].SetActive(false);
        }
    }

}
