using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToUI : MonoBehaviour
{
    public Button button;
    public GameObject uiToDisable;
    public GameObject uiToActivate;
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        uiToDisable.SetActive(false);
        uiToActivate.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
