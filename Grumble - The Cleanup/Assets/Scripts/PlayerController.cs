using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This is the script that manages the player. All work on the player goes inside of here.*/
public class PlayerController : MonoBehaviour
{
    //Components

    //Animation
    private BoxCollider2D BoxCollider;                //Creates a reference to the Box collider
    private float Box_X;                            //The size of the X (BoxCollider)
    private float Box_Y;                            //The size of the Y (BoxCollider)
    private Animator myAnimator;                    //Creates a reference to the annimator 
    public static bool attackAnimationActive;		//Whether or not the attack animation is active 

    [SerializeField]
    private AudioSource EnemyHitSound;              //The sound of the player Swinging Weapon

    //Checks
    public LayerMask whatIsGround;					//Defining what is ground
    public Transform groundCheck;                   //Checks if player is on the ground
    public float groundCheckRadius;                 //Area below the player to check for ground
    private bool grounded;                          //Is the player on the ground
    private bool facingRight;                       //Whether the player is facing right or not
    private bool isDead = false;                     //Whether or not the player is dead
    private bool canAttack = true;


    //Physics
    private Rigidbody2D myRigidbody;                //Creates a reference to the rigidbody
    private bool Jump = false;                      //Whether player can jump or not
    [SerializeField]
    private float movementSpeed;                    //The speed at which the player will move
    public float jumpHeight;						//The height the player will jump

    //Attributes
    private bool isAttacking;						//Whether the player is attacking or not
	[SerializeField]
    public static float health = 5;					//The health of the player 

	void Start ()
    {
		myRigidbody = GetComponent<Rigidbody2D> ();		//Grabs the rigidbody componenty from the player, (Same Reason as above)
		myAnimator = GetComponent<Animator> ();			//Grabs the Animator componenty from the player, (Same Reason as above)
		BoxCollider = GetComponent<BoxCollider2D> ();		//Grabs the BoxCollider componenty from the player, (Same Reason as above)

		Box_X = BoxCollider.size.x;						//Assigining the x size to the float (easier to reference)
		Box_Y = BoxCollider.size.y;						//Assigining the y size to the float (easier to reference)
	}
		

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);		//Less intensive than checking every frame, will check if the player is on the GROUND tag.
	}

	void Update ()
    {
		float horizontal = Input.GetAxis ("Horizontal");
        if (canAttack)
        {
            HandleMovement(horizontal);
            HandleInput();
            HandleAttacks();
            Flip(horizontal);
            ResetValues();
        }

        Death();
	}

    /*HandleMovement() Checks if the player is current in an attacking animation statre and will stop the player from sliding if so.
     Will also check if the player is trying to jump and, if player is grounded, will make the player's Y velocity increase via Force. */
	private void HandleMovement(float horizontal)
	{

		if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))							//if the player is not in the Attack Animation Phase
		{
            attackAnimationActive = false;																//Change the Active Animation bool to false
			myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, myRigidbody.velocity.y);	//Stops the player from Sliding
		}

		myAnimator.SetFloat ("Walking", Mathf.Abs(horizontal));											//sets the animation to walking


		if (Input.GetButton ("Jump") && grounded)														//If the Jump button has been pressed and player is on the ground
		{
			GetComponent<Rigidbody2D> ().AddForce (new Vector2(0, jumpHeight), ForceMode2D.Impulse);	//Will add a force decided in unity to the player moving them up
		}
	}

    /*Flip() Will flip the player's X value when they turn.*/
	private void Flip(float horizontal)										
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 							//if the player's Horizontal (movement) value is less than 0 will flip the player in that direction
		{
			facingRight = !facingRight;																	

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;																			//if its currently on the positive will change to negative, and vise versa

			transform.localScale = theScale;															//will set the value on the Player
		}
	}

    /*HandleAttacks() will play the attacking animation if the player is currently attacking and no animation
     for attacking is currently active. Will also set the box collider's size accordingly so that the walking stick can hit the enemies.*/
	private void HandleAttacks()
	{
		if (isAttacking && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))				//if the player is in attacking stage but animation not yet triggered
		{
			myAnimator.SetTrigger ("Attack");															//turns the animation on
			EnemyHitSound.Play ();																		//plays the attacking sound
			myRigidbody.velocity = Vector2.zero;														//stops the player from moving
		}

		if (this.myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) 							//once player is in the attacking Animation
		{
            attackAnimationActive = true;																//turn activeanimation to true
			Vector2 b_size = BoxCollider.size;															//increase the size of the box collider
			b_size = new Vector2 (6, Box_Y);
			BoxCollider.size = b_size;
		}
	}

    /*HandleInput() will set the isAttacking bool to true if they player is pressing their attack button.*/
    private void HandleInput()																			
    {
        if (Input.GetButtonDown("Attacking"))															//if the Attack Button has been pressed
        {
            isAttacking = true;																			//sets the attacking stage to true
        }
    }

    /*ResetValues() will check if an animation cycle has ended and the player is no longer attacking,
    if that is the case, it will reset the player's box collider.*/
	private void ResetValues()
	{
        isAttacking = false;																			//resets attacking stage to false

        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag ("NotAttack")) 						//if the player is no longer in attacking stage
		{
            Vector2 b_size = BoxCollider.size;															//reset the box collider size
			b_size = new Vector2 (Box_X, Box_Y);
			BoxCollider.size = b_size;
		}
	}

    /*Death() will check if the player is not currently dead, and it's health is less than or equal to 0,
     if true, it will set isDead to true and trigger the level ending via Loss UI to appear. Will also stop
     the player from being able to attack.*/
    void Death()
    {
        if (isDead == false)
        {
            if (health <= 0)                                                                                //if player health goes below 0
            {
                isDead = true;                                                                              //set is dead as true
                GameManager.instance.TriggerLevelEnd("Lose");
                canAttack = false;
            }
        }
    }

    /*TakeDamage() is called when a player is hit by an enemy. Will reduce the player's lives via
     the value that is passed in.*/
    public static void TakeDamage(float livesToTake)
    {
	    health -= livesToTake;   //livesToTake is equal to the damage passed in when this function is called. (via the EnemyManagement.cs script.)
		Debug.Log ("Player has lost " + livesToTake + " live[s]");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "LevelEnd")
        {
            Debug.Log("Player has reached the end");
            GameManager.instance.TriggerLevelEnd("Win");
        }
  
        if (col.gameObject.tag == "Collectable")
        {
            collectItems();
        } 

        if (col.gameObject.tag == "LevelVoid")
        {
            health = 0;
        }
    }

    void collectItems()
    {
        GameManager.instance.UpdateGrumbleCounter(3);
    }
}
