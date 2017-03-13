using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This is the script that manages the player. All work on the player goes inside of here.*/
public class PlayerController : MonoBehaviour
{
    //Components
    private Transform trans;

    //Attributes
    private static int lives = 5;
    private bool isDead = false;

	void Start ()
    {
        trans = GetComponent<Transform>();  //Grabs the players transform and assigns it locally. (Less intensive than grabbing it everytime).
	}
	void Update ()
    {
        Death();
	}

    void Death()
    {
        if (lives <= 0)
        {
            Debug.Log("Player has died..");
            isDead = true;
        }
    }

    /*Called when a player is hit by an enemy. Will reduce the player's lives.*/
    public static void TakeDamage(int livesToTake)
    {
        lives -= livesToTake;   //livesToTake is equal to the damage passed in when this function is called. (via the EnemyManagement.cs script.)
        Debug.Log("Player has lost " + livesToTake + " live[s]");
    }

}
