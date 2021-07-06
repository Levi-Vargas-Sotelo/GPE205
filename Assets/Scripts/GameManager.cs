using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // Reference the only instance of this class which is this same one in a variable
    public static GameManager instance;

    // Get the player game object
    public GameObject player;

    // Enemies list
    public List<GameObject> enemies;

    // Powerups list
    public List<GameObject> powerups;

    // Player Spawners list
    public List<GameObject> spawners;
    // Input for a random seed generator
    public int spawnSeed;

    public GameObject mapGenObject;
    public MapGenerator mapGen;

    // Awake is called when the GameObject is initialized 
    public void Awake() 
    {
        // Make sure there is only one instance of this class
        if (instance == null) 
        {
            // Instance it if there is none in the scene
            instance = this;
        } else 
        {
            // Print an error log and destroy the instance since there are multiple game managers
            Destroy(gameObject);
            Debug.LogError("There are more than one game managers");
        }

        mapGenObject = GameObject.Find("Map Generator");
        mapGen = mapGenObject.GetComponent<MapGenerator>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mapGen.MakeMap();

        // Set the random value and start the spawn tank function
        spawnSeed = DateToInt (DateTime.Now);
        UnityEngine.Random.InitState(spawnSeed);

        // Find all enemy tanks in scene and add them to the Enemies list
        foreach(GameObject tank in GameObject.FindGameObjectsWithTag("Tank")) 
        {
            enemies.Add(tank);
        }

        // Find all pickups tanks in scene and add them to the powerups list
        foreach(GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup")) 
        {
            powerups.Add(pickup);
        }

        // Find all pickups tanks in scene and add them to the powerups list
        foreach(GameObject point in GameObject.FindGameObjectsWithTag("Spawner")) 
        {
            spawners.Add(point);
        }

        // Spawn tank function after making the list
        SpawnTank ();

        // Finds the player object to keep track of it
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void LookforPlayer ()
    {
        // Finds the player object to keep track of it
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FindTanks ()
    {
        // Find all enemy tanks in scene and add them to the Enemies list
        foreach(GameObject tank in GameObject.FindGameObjectsWithTag("Tank")) 
        {
            enemies.Add(tank);
        }

        // Finds the player object to keep track of it
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FindSpawns ()
    {
        // Find all pickups tanks in scene and add them to the powerups list
        foreach(GameObject point in GameObject.FindGameObjectsWithTag("Spawner")) 
        {
            spawners.Add(point);
        }

        SpawnTank ();
        LookforPlayer();

        // Finds the player object to keep track of it
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FindPickups ()
    {
        // Find all pickups tanks in scene and add them to the powerups list
        foreach(GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup")) 
        {
            powerups.Add(pickup);
        }
    }

    public void SpawnTank ()
    {
        if (spawners.Count == 0)
        {
            Debug.Log("Theres no spawners");
        }
        else
        {
            //Spawn tank after getting all the possible spawn points
            int randomIndex = UnityEngine.Random.Range(0, spawners.Count);
            GameObject Point = spawners[randomIndex]; 
            PlayerSpawn spawnPoint = Point.GetComponent<PlayerSpawn>();
            spawnPoint.SpawnTank();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            SpawnTank();
            LookforPlayer();
            Debug.Log("Player dead");
        }

        if (enemies.Count == 0)
        {
            FindTanks();
        }

        if (spawners.Count == 0)
        {
            FindSpawns();
            // Finds the player object to keep track of it
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public GameObject RandomSpawnPoint () 
    {
        // Get a random tank function
        return spawners[UnityEngine.Random.Range(0,spawners.Count)];
    }



    public int DateToInt (DateTime dateToUse) 
    {
        // We add all the current date and time values to use it as an integer
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
