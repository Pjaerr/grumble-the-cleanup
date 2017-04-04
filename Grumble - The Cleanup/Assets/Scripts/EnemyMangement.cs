using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMangement : MonoBehaviour
{
    //Components
    [SerializeField]
    private Transform[] movementPoints; //The points which the enemy will move between at random.
    private Transform trans;            //The enemy's transform.
    [SerializeField]
    private AudioSource playerHitSound; //The audio source containing the player hit sound. Assigned in inspector.
    [SerializeField]
    private AudioSource EnemyPainSound;
    public GameObject DeathParticles;   //Object used to spawn particles on death.

    //Attributes
    [SerializeField]
    private float movementSpeed;    //The speed at which the enemy will move in Unity Units. Assigned in the inspector.
    private bool hasMoved = true;   //The check to see if the enemy is at its target point. (See Movement()).
    private int r;                  //The random number generated to choose a point to move to. (See Movement()).
    [SerializeField]
    private float enemyDmg = 1;     //The enemy's damage.
    [SerializeField]
    private float playerDmg = 1;    //Damage the player will inflict onto this enemy object.
	[SerializeField]
	private float enemyHealth = 2;  //The health the enemy has.
	private bool isDead = false;    //True if the enemy has run out of health.


    void Start()
    {
        trans = GetComponent<Transform>();  //Assigning the player's transform once, instead of having to get component every time.
    }
    void Update()
    {
        Movement();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            CheckDeath();

            if (PlayerController.attackAnimationActive) //If the player is currently attacking.
            {
                EnemyPainSound.Play();  //Play enemy getting hit sound.
                TakeDamage(playerDmg);  //Pass the amount of dmg the player does to this object to its TakeDamage function.
            }
            else if (isDead == false)
			{
                playerHitSound.Play();  //Plays the playerHitSound assigned in the inspector on the Enemy object.
                PlayerController.TakeDamage(enemyDmg);   //If this enemy hits a player. Call the Player's TakeDamage() function and pass in the damage this enemy does.
			}

           
        }
    }

    /*Move() will move the enemy object it is attached to, to random points within the movementPoints array (assigned in inspector)
    Once it reaches a point, it will choose another point at random and repeat.*/
    void Movement()
    {
        /*Checks to see if the enemy has reached its first point. Intially set to true to make sure
        that a random point is chosen at least once before the enemy has moved.*/
        if (hasMoved)
        {
             r = Random.Range(0, movementPoints.Length);
        }

        float timeStep = movementSpeed * Time.deltaTime;    //Smoothing out the movementSpeed by the time passed in the previous frame.
        trans.position = Vector2.MoveTowards(trans.position, movementPoints[r].position, timeStep); //Moving the object towards the movementPoint.
        //Debug.Log("Enemy Moving to point " + movementPoints[r].position);

        /*If the enemy has reached its point, sets hasMoved to true to make sure another
        random point can be chosen. If it has not yet reached its point, it sets hasMoved
        to false to avoid a new random point being chosen before it has finished moving.*/
        if (trans.position == movementPoints[r].position)
        {
            hasMoved = true;
        }
        else
        {
            hasMoved = false;
        }
    }


	void CheckDeath()
	{
		if (enemyHealth <= 0) 
		{
			Debug.Log ("Enemy Has Been Killed");
			isDead = true;
            GameManager.instance.UpdateGrumbleCounter(10);
            if (gameObject.tag == "Enemy") 
			{
				Instantiate (DeathParticles, trans.position, Quaternion.identity);
				DeathParticles.transform.parent = null;
                Destroy(gameObject);
			}
		}
	}


	private void TakeDamage(float livesToTake)
	{
		enemyHealth -= livesToTake;
		Debug.Log("Enemy Has Lost " + livesToTake + " live[s]");
	}


}
