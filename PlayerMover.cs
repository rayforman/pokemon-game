using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMover : MonoBehaviour
{
    Vector2 moveVector;
    public float moveSpeed = 8.0f;
    private Vector3 ogPos;

    public void movePlayer(InputAction.CallbackContext _context)        // Updates vectors on events
    {
        moveVector = _context.ReadValue<Vector2>();
    }

    void Start()
    {
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Updating");
        // Move Player
        Vector3 movement = new Vector3(moveVector.x, 0, moveVector.y);
        movement.Normalize();
        transform.Translate(moveSpeed * movement * Time.deltaTime);

        // Respawn if clipped through the map
        if (transform.position.y < -50f)
            transform.position = ogPos;
    }
}
