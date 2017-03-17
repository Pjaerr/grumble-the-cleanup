using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This is the script that manages the player. All work on the player goes inside of here.*/
public class PlayerController : MonoBehaviour
{
    //Components
    private Transform trans;

	private Rigidbody2D myRigidbody;
	private bool facingRight;

	private Animator myAnimator; //animation

	[SerializeField]
	private float movementSpeed;//movement
	public float jumpHeight;	//jump height

	public Transform groundCheck;
	public float groundCheckRadius;	//ground check
	public LayerMask whatIsGround;
	private bool grounded;
	private bool Jump = false;

	private bool attack;
	private BoxCollider2D BCollider;
	private float Box_X;
	private float Box_Y;

	[SerializeField]
	private int Dmg = 1;
	[SerializeField]
	private AudioSource EnemyHitSound;
	[SerializeField]
	private AudioSource EnemyPainSound;



    //Attributes
	[SerializeField]
   private static int lives = 5;
   private bool isDead = false;

	void Start ()
    {
       trans = GetComponent<Transform>();  //Grabs the players transform and assigns it locally. (Less intensive than grabbing it everytime).

		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
		BCollider = GetComponent<BoxCollider2D> ();
		Box_X = BCollider.size.x;
		Box_Y = BCollider.size.y;
	}
		

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}



	void Update ()
    {
		float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement(horizontal);
		HandleInput ();
		HandleAttacks ();
		Flip (horizontal);
		ResetValues ();
        Death();
	}

	private void HandleMovement(float horizontal)
	{

		if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, myRigidbody.velocity.y);
		}

		myAnimator.SetFloat ("Walking", Mathf.Abs(horizontal));


		if (Input.GetButton ("Jump") && grounded)
		{
			GetComponent<Rigidbody2D> ().AddForce (new Vector2(0, jumpHeight), ForceMode2D.Impulse);
		}
	}

	private void Flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 
		{
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;

			transform.localScale = theScale;
		}
	}

	private void HandleAttacks()
	{
		if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			myAnimator.SetTrigger ("Attack");
			EnemyHitSound.Play ();
			myRigidbody.velocity = Vector2.zero;
		}

		if (this.myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) 
		{
			Vector2 b_size = BCollider.size;
			b_size = new Vector2 (6, Box_Y);
			BCollider.size = b_size;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy") 
		{
			
			if (this.myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) 
			{
				EnemyPainSound.Play ();
				EnemyMangement.TakeDamage (Dmg);
			}
		}
	}

	private void HandleInput()
	{
		if (Input.GetButtonDown ("Attacking"))
		{
			attack = true;
		}
	}



	private void ResetValues()
	{
		attack = false;

		if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag ("NotAttack")) 
		{
			Vector2 b_size = BCollider.size;
			b_size = new Vector2 (Box_X, Box_Y);
			BCollider.size = b_size;
		}
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
		
		if (!GameObject.Find("Player").GetComponent<PlayerController>().attack) 
		{
			lives -= livesToTake;   //livesToTake is equal to the damage passed in when this function is called. (via the EnemyManagement.cs script.)
			Debug.Log ("Player has lost " + livesToTake + " live[s]");
		}
    }

}
