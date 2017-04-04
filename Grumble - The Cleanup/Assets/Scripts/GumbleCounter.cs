using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//Accesses the ScoreUpdate int from Enemy Management.cs
public class Accessor : MonoBehaviour
{

    void Start()
    {
        GameObject Enemies = GameObject.Find("Enemies");
        EnemyMangement enemyManagement = Enemies.GetComponent<EnemyMangement>();
        enemyManagement.ScoreUpdate =+ 10;
    }
}

public class GumbleCounter : MonoBehaviour
{

    public Text ScoreText;
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
