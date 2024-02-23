using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
    public Button button;
    public Button prButton;
    public Camera camera;
    public Camera prCamera;
    public GameObject ui;
    public GameObject prUI;
    
    // Start is called before the first frame update
    void Start()
    {
        // Default camera is in world
        camera.enabled = true;
        prCamera.enabled = false;

         // Default ui is in world
        ui.SetActive(true);
        prUI.SetActive(false);

        // Add button listener, both map to same function
        button.onClick.AddListener(OnButtonClick);
        prButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Toggle ui in use
        ui.SetActive(!ui.activeSelf);
        prUI.SetActive(!prUI.activeSelf);

        // Toggle camera in use
        camera.enabled = !camera.enabled;
        prCamera.enabled = !prCamera.enabled;
    }
}
