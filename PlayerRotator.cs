using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerRotator : MonoBehaviour, IDragHandler
{
    public Transform cameraTransform;
    public float lookSpeed = 0.1f;    
    private Vector2 lookVector;
    private float cameraPitch;
    private float deltaX;
    private float deltaY;

    public void OnDrag(PointerEventData eventData)
    {
        deltaX = eventData.delta.x;
        deltaY = eventData.delta.y;
        RotatePlayer(deltaX, deltaY);
        // Debug.Log("OnDrag called!");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Updating");
        // Rotate player: up and down
        // cameraPitch = Mathf.Clamp(cameraPitch - lookVector.y, -90.0f, 90.0f);
        // cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        // // Rotate player: left and right
        // transform.Rotate(transform.up, lookVector.x);
    }

    private void RotatePlayer(float deltaX, float deltaY)
    {
        cameraPitch = Mathf.Clamp(cameraPitch - deltaY, -120.0f, 120.0f);     // limits tilt
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch*lookSpeed, 0, 0);
        transform.Rotate(Vector3.up, deltaX * lookSpeed);
    }
}
