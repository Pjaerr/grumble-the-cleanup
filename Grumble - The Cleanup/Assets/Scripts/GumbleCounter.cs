using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GumbleCounter : MonoBehaviour
{

    public int score;
    public GUIText ScoreText;
    [SerializeField]
    private int ScoreValue;
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

    }

    //Function to update the score and display it
    public void UpdateScoreValue(int scoreUpdate)
    {
        ScoreValue += scoreUpdate;

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