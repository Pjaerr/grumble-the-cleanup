using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This is the script that manages the player. All work on the player goes inside of here.*/
public class PlayerController : MonoBehaviour
{
    //Components
    //private Transform trans;
	private Rigidbody2D myRigidbody;
	private bool facingRight;

	private Animator myAnimator;

	[SerializeField]
	private float movementSpeed;
	public Vector2 jumpHeight;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;
	private bool Jump = false;

    //Attributes
   private static int lives = 5;
   private bool isDead = false;

	void Start ()
    {
       //trans = GetComponent<Transform>();  //Grabs the players transform and assigns it locally. (Less intensive than grabbing it everytime).

		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
	}
		

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}



	void Update ()
    {
		float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement(horizontal);

		Flip (horizontal);





        Death();
	}

	private void HandleMovement(float horizontal)
	{
		myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, myRigidbody.velocity.y);

		myAnimator.SetFloat ("LeftWalk", Mathf.Abs(horizontal));


		if (Input.GetButton ("Jump") && grounded)
		{
			GetComponent<Rigidbody2D> ().AddForce (jumpHeight, ForceMode2D.Impulse);
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
