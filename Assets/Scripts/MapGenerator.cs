using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    // Variables to know the size of the grid of tile rooms we are making
    public int rows;
    public int cols;
    private float roomWidth = 50.0f;
    private float roomHeight = 50.0f;

    // Input for a random seed generator
    public int mapSeed;  

    // An array list of the tile rooms we have for the script to choose and make a map out of
    public GameObject[] gridPrefabs;

    // Array to reference the rooms we created to access them and change them
    private Room[,] grid;

    // Use map of the day seed
    public bool MapOfTheDay;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeMap()
    {
        // If we set it to the map of the day then use only today values of the seed
        if (MapOfTheDay) 
        {
            mapSeed = DateToInt (DateTime.Now.Date);
            GenerateGrid ();
        } 
        else 
        {
            mapSeed = DateToInt (DateTime.Now);
            // Creates the grid for the map at start of the game
            GenerateGrid ();
        }
    }

    // This function just returns a random prefab from the array list
    public GameObject RandomRoomPrefab () 
    {
        return gridPrefabs[UnityEngine.Random.Range(0,gridPrefabs.Length)];
    }

    // Function to generate the grid using nested for each statements which means there is a loop inside another to make the grid making more easy
    public void GenerateGrid() 
    {
        // Set the random value using the seed we set
        UnityEngine.Random.InitState(mapSeed);

        // This clears the grid 
        grid = new Room[cols,rows];

        // For each row in the grid
        for (int r = 0; r < rows; r++) 
        {
            // for each column in the same row
            for (int c = 0; c < cols ; c++) 
            {
                // Find out the location by multiplying it with the variables from before
                float xPosition = roomWidth * c;
                float zPosition = roomHeight * r;
                Vector3 newPosition =  new Vector3 (xPosition,0.0f,zPosition);
                            
                // Instantiate a random room using the function from before and place it in the place calculated from before
                GameObject tempRoomObj = Instantiate (RandomRoomPrefab(),newPosition,Quaternion.identity) as GameObject;

                // Parent the room to this map generator object
                tempRoomObj.transform.parent = this.transform;

                // Name it depending on the room and column it is at
                tempRoomObj.name = "Room_"+c+","+r;

                // Obtain the room and store it
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // Open the north doors by setting them inactive on the last row rooms
                if (r == 0) 
                {
                    tempRoom.doorNorth.SetActive(false);
                } 
                else if ( r == rows-1 )
                {
                    // Open the south door if this is the top row
                    tempRoom.doorSouth.SetActive(false);
                }
                else 
                {
                    // If it is at neither of those places then open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                
                // Open the East door if it is on the first column
                if (c == 0) 
                {
                    tempRoom.doorEast.SetActive(false);
                } 
                else if ( c == cols-1 )
                {
                    // Open the West door if this is the last column
                    tempRoom.doorWest.SetActive(false);
                }
                else 
                {
                    // Open both doors if it is at neither of these places
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                
                // Store it on the array for the grid
                grid[c,r] = tempRoom;
            }
        }
    }

    // Function to know the date and time by using the System library
    public int DateToInt (DateTime dateToUse) 
    {
        // We add all the current date and time values to use it as an integer
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
