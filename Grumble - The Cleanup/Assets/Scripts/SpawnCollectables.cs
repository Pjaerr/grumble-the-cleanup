using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectables : MonoBehaviour {


    // This script will manage the spawning of the collectables. 

    // Components: 

    // This creates a public variable for the prefab to clone.

    [SerializeField]
    private GameObject spawnedObject;

        // This gets the possition of the spawn point allowing the object to be spawned at the correct location.

    private Vector3 spawnposition;
    private Quaternion spawnRotation;

        // This prevents the spawning action to happen more than once. 

    private bool spawnObject;

	// Use this for initialization
	void Start () {

        // This gets the possition of the spawn point at the beggining of the game.

        spawnposition = gameObject.transform.position;
        spawnRotation = gameObject.transform.rotation;

        // This allows for the spawning action to happen.

        spawnObject = true;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (spawnObject == true)
        {
            // This spawns the collectable at the possition the object this script is connected to.

            GameObject.Instantiate(spawnedObject, spawnposition, spawnRotation);

            // This stops the item from being spawned multiple times. 

            spawnObject = false;
        }

	}
}
