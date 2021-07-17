using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    // Reference the only instance of this class which is this same one in a variable
    public static SaveManager saverManager;

    public Text textToSave;
    public Text scores;

    public void Awake() 
    {
        // Make sure there is only one instance of this class
        if (saverManager == null) 
        {
            // Instance it if there is none in the scene
            saverManager = this;
        } else 
        {
            // Print an error log and destroy the instance since there are multiple game managers
            Destroy(gameObject);
            Debug.LogError("There are more than one save manager");
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

    public void Save () 
    {
        
        PlayerPrefs.Save ();
    }

    public void Load ()
    {
        //GameManager.instance.menus.musicVolume = PlayerPrefs.GetFloat ("Vol");
        //GameManager.instance.menus.sFXVolume = PlayerPrefs.GetFloat ("SFX");
    }
}
