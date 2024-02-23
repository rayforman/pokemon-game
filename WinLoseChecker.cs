using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseChecker : MonoBehaviour
{
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    
    // For Win Condition
    public bool caughtBug = false;
    public bool caughtFly = false;
    public bool caughtFish = false;
    private bool isWon = false;

    // For Lose Condition
    public bool isDead = false;
    private bool isLost = false;
    
    void Start()
    {
        playUI.SetActive(true);
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

    void Update()
    {
        if (!isWon && caughtBug && caughtFly && caughtFish)
            WinGame();
        else if (!isLost && isDead)
            LoseGame();
    }

    public void WinGame()
    {
        isWon = true;

        Debug.Log("In WinGame function");
        if (playUI != null && winUI != null)
        {
            playUI.SetActive(false);
            winUI.SetActive(true);
            Debug.Log("You Won!");
        }
    }
    public void LoseGame()
    {
        isLost = true;
        
        Debug.Log("In LoseGame function");
        if (playUI != null && winUI != null)
        {
            playUI.SetActive(false);
            loseUI.SetActive(true);
            Debug.Log("You Lost!");
        }
    }
}
