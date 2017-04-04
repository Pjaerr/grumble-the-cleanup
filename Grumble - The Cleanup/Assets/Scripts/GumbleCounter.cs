using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GumbleCounter : MonoBehaviour
{

    public Text ScoreText;
    private int ScoreValue = 0;
    [SerializeField]


    // Use this for initialization
    void Start()
    {
        ScoreValue = 0;
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText();
    }

    //Function to update the score and display it
    public void UpdateScoreValue(int ScoreUpdate)
    {
        ScoreValue += ScoreUpdate;

        UpdateScoreText();
    }

    //Shows the updated score in game.
    void UpdateScoreText()
    {
        ScoreText.text = "Grumble Counter: " + ScoreValue;
    }

    //Resets the score
    void ResetScore()
    {
        ScoreValue = 0;
        UpdateScoreText();
    }
}
