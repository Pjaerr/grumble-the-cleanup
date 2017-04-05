using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool loadHud = false;

    //UI
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
    [SerializeField]
    GameObject pauseMenu;

    //UI Values
    private float playerHealthValue = 0;
    private int timerValue = 0;
    private float grumbleCounterValue = 0;

    /*Creates a static instance, of which there can only be one, this allows access to this scripts' 
    public variables and functions inside of other scripts.*/
    public static GameManager instance;

    void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        Singleton();
    }

    void Start()
    {
        /*Checks to see if loadHud is true, to avoid trying to load UI that doesn't exist when on the main menu*/
        if (loadHud)
        {
            HUD.SetActive(true);
            LoadHUD();
        }

    }
    void Update()
    {
        if (loadHud)
        {
            LoadHUD();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    void LoadHUD()
    {

        UpdateHUDValues();

        playerHealth.text = "Health: " + playerHealthValue.ToString();
        timer.text = "Time: " + timerValue.ToString();
        grumbleCounter.text = "Grumble Counter: " + grumbleCounterValue.ToString();

    }
    
    public void UpdateGrumbleCounter(int amount)
    {
        grumbleCounterValue += amount;
    }

    void UpdateHUDValues()
    {
        /*Set the relevant values (Player health and Timer) here to what you need them to be and they should update on the HUD
         automatically as long as the values themselves are being changed here.*/
        timerValue += 1;

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

        Time.timeScale = 0;
        
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Restart(bool restart)
    {
        if (restart)
        {
            LevelEndLoseUI.SetActive(false);
            LevelEndWinUI.SetActive(false);
            Time.timeScale = 1;
            PlayerController.health = 5;
            SceneManager.LoadScene(1);
        }
        else
        {
            Application.Quit();
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
