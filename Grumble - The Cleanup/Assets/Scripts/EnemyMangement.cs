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

    //Attributes
    [SerializeField]
    private float movementSpeed;    //The speed at which the enemy will move in Unity Units. Assigned in the inspector.
    private bool hasMoved = true;   //The check to see if the enemy is at its target point. (See Movement()).
    private int r;                  //The random number generated to choose a point to move to. (See Movement()).
    [SerializeField]
    private int dmg = 1;           //The enemy's damage, measured in lives it takes.

	[SerializeField]
	private static int EnemyHealth = 2;
	private bool Death = false;
	public GameObject DeathParticles;

    void Start()
    {
        trans = GetComponent<Transform>();
    }
    void Update()
    {
        Movement();
		Dead ();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
			if (Death == false)
			{
            playerHitSound.Play();  //Plays the playerHitSound assigned in the inspector on the Enemy object.
            PlayerController.TakeDamage(dmg);   //If this enemy hits a player. Call the Player's TakeDamage() function and pass in the damage this enemy does.
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


	void Dead()
	{
		if (EnemyHealth <= 0) 
		{
			Debug.Log ("Enemy Has Been Killed");
			Death = true;
			if (gameObject.name == "Temp-Enemy") 
			{
				Instantiate (DeathParticles, transform.position, Quaternion.identity);
				DeathParticles.transform.parent = null;
				Destroy (gameObject);
			}
		}
	}


	public static void TakeDamage(int livesToTake)
	{
		EnemyHealth -= livesToTake;
		Debug.Log("Enemy Has Lost " + livesToTake + " live[s]");
	}


}
