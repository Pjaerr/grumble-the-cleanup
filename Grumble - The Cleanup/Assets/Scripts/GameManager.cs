using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool loadHud = false;

    [SerializeField]
    GameObject mainMenuUI;

    [SerializeField]
    GameObject LevelEndWinUI;

    [SerializeField]
    GameObject LevelEndLoseUI;

    [SerializeField]
    GameObject HUD;

    [SerializeField]
    Text playerHealth;
    [SerializeField]
    Text timer;
    [SerializeField]
    Text grumbleCounter;

    private float playerHealthValue = 0;
    private float timerValue = 0;
    private float grumbleCounterValue = 0;


    void Start()
    {
        if (loadHud)
        {
            LoadHUD();
        }
        
    }
    void Update()
    {
        if (loadHud)
        {
            LoadHUD();
        }

    }

    void LoadHUD()
    {

        UpdateHUDValues();

        playerHealth.text = "Health: " + playerHealthValue.ToString();
        timer.text = "Time: " + timerValue.ToString();
        grumbleCounter.text = "Grumble Counter: " + grumbleCounterValue.ToString();

    }
    void UpdateHUDValues()
    {
        //TO THOMAS KIRKLAND********
        /*Set the relevant values (Grumble counter and Timer) here to what you need them to be and they should update on the HUD
         automatically as long as the values themselves are being changed here.*/


        playerHealthValue = PlayerController.health;
    }

    /*Call this function and pass in relevant string to activate the level end UI.*/
    public void TriggerLevelEnd(string type)
    {
        HUD.SetActive(false);

        if (type == "Win")
        {
            LevelEndWinUI.SetActive(true);
        }
        else if (type == "Lose")
        {
            LevelEndLoseUI.SetActive(true);
        }
        
    }

    public void MainMenuControl(bool play)
    {
        if (play)
        {
            mainMenuUI.SetActive(false);
            SceneManager.LoadScene(1);
        }
        else
        {
            Application.Quit();
        }
    }
}
