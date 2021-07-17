using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // Reference the only instance of this class which is this same one in a variable
    public static GameManager instance;

    // Get the player game object
    public GameObject playerController;

    public bool gameStart;

    // To know if the game is over and players lost their lives
    public bool gameOver;

    // Input for a random seed generator
    public int spawnSeed;

    // Menu variable to reference the functions from it
    public Menus menus;
    public float music;
    public float sFX;

    // Enemies list
    public List<GameObject> enemies;

    // Powerups list
    public List<GameObject> powerups;

    // Player Spawners list
    public List<GameObject> spawners;

    // Make list of the scores
    public List<ScoreData> scores;

    // List to hold the current players
    public List<InputController> playersList;

    

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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void StartOnePlayer ()
    {
        MapGenerator.mapGen.MakeMap();

        // Set the random value and start the spawn tank function
        spawnSeed = DateToInt (DateTime.Now);
        UnityEngine.Random.InitState(spawnSeed);

        SpawnInputController ();

        gameStart = true;
    }

    public void StartTwoPlayer ()
    {
        Debug.Log("starting 2 player mode");

        MapGenerator.mapGen.MakeMap();

        // Set the random value and start the spawn tank function
        spawnSeed = DateToInt (DateTime.Now);
        UnityEngine.Random.InitState(spawnSeed);

        SpawnFirstPlayerInputController ();

        SpawnSecondPlayerInputController ();

        gameStart = true;
    }

    public void SpawnTank (InputController controllerWhoSpawned)
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
            spawnPoint.SpawnTank (controllerWhoSpawned);
            
        }
    }

    public void SpawnInputController ()
    {
        playerController = Instantiate (playerController) as GameObject;
    }

    public void SpawnFirstPlayerInputController ()
    {
        playerController = Instantiate (playerController) as GameObject;
        InputController pContr = playerController.GetComponent<InputController>();
        
        pContr.AdjustCameraUp();
    }

    public void SpawnSecondPlayerInputController ()
    {
        playerController = Instantiate (playerController) as GameObject;
        InputController pContr = playerController.GetComponent<InputController>();
        pContr.SecondPlayer = true;
        pContr.AdjustCameraDown();
    }

    // Update is called once per frame
    void Update()
    {
        sFX = menus.sFXVolume;
        music = menus.musicVolume;
    }

    public void CheckPlayerLives()
    {
        if (gameStart)
        {
            foreach (InputController players in playersList)
            {
                if (players.playerLives == 0)
                {
                    gameOver = true;
                    gameStart = false;
                    GameIsOver();
                }
            }
        }
    }

    public void GameIsOver()
    {
        menus.GameDone();
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
