using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectedCollectable : MonoBehaviour {

    // This will be used to gain access to the game manager and its script

    [SerializeField]
    private GameObject gameManager;

    // This will be used to call public functions from within the game manager class.

    private GameManager newScore;

    // Use this for initialization
    void Start () {

        // Finds the one item with the GameManager tag and assigns it to a local variable.

        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        // uses the game object it found in a preveous step and gets a script connected to it.

        newScore = gameManager.GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This calls a script connected to the GameManager which will increase the player's Grumble Counter.

        newScore.increaseScore();
        
        // This then destroys the game object the script is connected to. 

        Destroy(gameObject);
    }
}
