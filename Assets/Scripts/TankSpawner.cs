using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankSpawner : MonoBehaviour
{
    // List of tanks to spawn
    public GameObject[] tanksToSpawn;

    // Input for a random seed generator
    public int tankSeed;

    // Variable for the game manager
    public GameObject gameManagerObject;
    public GameManager gameManager;

    void Awake ()
    {
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the random value and start the spawn tank function
        tankSeed = DateToInt (DateTime.Now);
        UnityEngine.Random.InitState(tankSeed);
        SpawnTank (); 
          
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTank ()
    {
        // Spawn a random tank
        GameObject tank = Instantiate (RandomTankPrefab(),transform.position,Quaternion.identity) as GameObject;
    }

    public GameObject RandomTankPrefab () 
    {
        // Get a random tank function
        return tanksToSpawn[UnityEngine.Random.Range(0,4)];
    }

    public int DateToInt (DateTime dateToUse) 
    {
        // We add all the current date and time values to use it as an integer
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
