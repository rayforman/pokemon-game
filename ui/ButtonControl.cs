using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public Button button;
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    private string name;
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        name = button.name;
    }

    void Update()
    {
        Debug.Log("name");
    }

    void OnButtonClick()
    {
        Debug.Log("Button Clicked: " + name);
        if (name == "PlayAgainButton")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (name == "CloseButton")
        {
            winUI.SetActive(false);
            loseUI.SetActive(false);
            playUI.SetActive(true);
        }
        else if (name == "ExitButton")
            Application.Quit();
    }
}
