using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This is the script that manages the player. All work on the player goes inside of here.*/
public class PlayerController : MonoBehaviour
{
    //Components
    private Transform trans;

	private Rigidbody2D myRigidbody;				//Creates a reference to the rigidbody
	private bool facingRight;						//Whether the player is facing right or not

	private Animator myAnimator; 					//Creates a reference to the annimator 

	[SerializeField]
	private float movementSpeed;					//The speed at which the player will move
	public float jumpHeight;						//The height the player will jump

	public Transform groundCheck;					//Checks if player is on the ground
	public float groundCheckRadius;					//Area below the player to check for ground
	public LayerMask whatIsGround;					//Defining what is ground
	private bool grounded;							//Is the player on the ground
	private bool Jump = false;						//Whether player can jump or not
    private bool isAttacking;						//Whether the player is attacking or not
    public static bool attackAnimationActive;		//Whether or not the attack animation is active 


    private BoxCollider2D BCollider;				//Creates a reference to the Box collider
	private float Box_X;							//The size of the X (BoxCollider)
	private float Box_Y;							//The size of the Y (BoxCollider)

	[SerializeField]
	private float dmg = 1;							//The damage that the player does
    [SerializeField]
    private AudioSource EnemyHitSound;				//The sound of the player Swinging Weapon

    //Attributes
	[SerializeField]
   public static float health = 5;					//The health of the player 
   private bool isDead = false;						//Whether or not the player is dead

	void Start ()
    {
      	trans = GetComponent<Transform>();  			//Grabs the players transform and assigns it locally. (Less intensive than grabbing it everytime).
		myRigidbody = GetComponent<Rigidbody2D> ();		//Grabs the rigidbody componenty from the player, (Same Reason as above)
		myAnimator = GetComponent<Animator> ();			//Grabs the Animator componenty from the player, (Same Reason as above)
		BCollider = GetComponent<BoxCollider2D> ();		//Grabs the BoxCollider componenty from the player, (Same Reason as above)
		Box_X = BCollider.size.x;						//Assigining the x size to the float (easier to reference)
		Box_Y = BCollider.size.y;						//Assigining the y size to the float (easier to reference)
	}
		

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);		//Less intensive than checking every frame, will check if the player is on the GROUND tag.
	}

	void Update ()
    {
		float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement(horizontal);
        HandleInput();
        HandleAttacks();
		Flip (horizontal);
		ResetValues ();
        Death();
	}

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
			Vector2 b_size = BCollider.size;															//increase the size of the box collider
			b_size = new Vector2 (6, Box_Y);
			BCollider.size = b_size;
		}
	}

    private void HandleInput()																			
    {
        if (Input.GetButtonDown("Attacking"))															//if the Attack Button has been pressed
        {
            isAttacking = true;																			//sets the attacking stage to true
        }
    }

	private void ResetValues()
	{

        isAttacking = false;																			//resets attacking stage to false

        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag ("NotAttack")) 						//if the player is no longer in attacking stage
		{
            Vector2 b_size = BCollider.size;															//reset the box collider size
			b_size = new Vector2 (Box_X, Box_Y);
			BCollider.size = b_size;
		}
	}

    void Death()
    {
        if (health <= 0)																				//if player health goes below 0
        {
            Debug.Log("Player has died..");
            isDead = true;																				//set is dead as true
            GameManager.instance.TriggerLevelEnd("Lose");
        }
    }

    /*Called when a player is hit by an enemy. Will reduce the player's lives.*/
    public static void TakeDamage(float livesToTake)
    {
	    health -= livesToTake;   //livesToTake is equal to the damage passed in when this function is called. (via the EnemyManagement.cs script.)
		Debug.Log ("Player has lost " + livesToTake + " live[s]");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Player has reached the end");
        GameManager.instance.TriggerLevelEnd("Win");
    }
}
