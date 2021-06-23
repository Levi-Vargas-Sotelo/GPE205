using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Reference the only instance of this class which is this same one in a variable
    public static GameManager instance;

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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
