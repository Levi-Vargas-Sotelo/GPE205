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
    private TankData playerData;

    public float playerScore;

    public GameObject playerCam;

    public bool gameStart;
    // To know if the game is over and players lost their lives
    public bool gameOver;

    // The player's lives
    public float lives;

    public int spawnSeed;

    // Reference the map generator to use the functions in it
    public GameObject mapGenObject;
    public MapGenerator mapGen;

    // Enemies list
    public List<GameObject> enemies;

    // Powerups list
    public List<GameObject> powerups;

    // Player Spawners list
    public List<GameObject> spawners;
    // Input for a random seed generator

    // Make list of the scores
    public List<ScoreData> scores;

    // List to hold the current players
    public List<GameObject> playersList;

    

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

        gameStart = false;
        mapGenObject = GameObject.Find("Map Generator");
        mapGen = mapGenObject.GetComponent<MapGenerator>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void StartOnePlayer ()
    {
        mapGen.MakeMap();

        // Set the random value and start the spawn tank function
        spawnSeed = DateToInt (DateTime.Now);
        UnityEngine.Random.InitState(spawnSeed);

        // Find all pickups tanks in scene and add them to the powerups list
        foreach(GameObject point in GameObject.FindGameObjectsWithTag("Spawner")) 
        {
            spawners.Add(point);
        }

        FindSpawns ();

        // Spawn tank function after making the list
        SpawnTank ();

        // Finds the player object to keep track of it
        player = GameObject.FindGameObjectWithTag("Player");
        playerData = player.GetComponent<TankData>();
        playerScore = playerData.Score;

        gameStart = true;

        GiveCamera (player);
    }

    public void LookforPlayer ()
    {
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

    public void GiveCamera (GameObject tank)
    {
        playerCam = Instantiate (playerCam) as GameObject;
        CameraController camCont = playerCam.GetComponent<CameraController>();
        camCont.player = tank;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (lives >= 1)
            {
                if (player == null)
                {
                    lives -= 1;
                    SpawnTank();
                    LookforPlayer();
                    Debug.Log("Player dead");
                }
                else
                {
                    playerScore = playerData.Score;
                }
            } else 
            {
                
            }
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

    // Sort of the scores and then reverse them so theyre the top 3 only and store them
    public void SortScores ()
    {
        scores.Sort();
        scores.Reverse();
        scores = scores.GetRange(0,3);
    }
}
