using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    // Variables for getting the menus in the canvas
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject gameOver;
    public GameObject uI;

    // Lives and score variables
    public Text livesA;
    public Text scoreA;



    // Get the mape generator object
    public MapGenerator mapGener;

    // Start is called before the first frame update
    void Start()
    {
        mapGener = GameManager.instance.mapGen;
    }

    // Update is called once per frame
    void Update()
    {
        livesA.text = "Lives: " + GameManager.instance.lives.ToString();
        scoreA.text = "Score: " + GameManager.instance.playerScore.ToString();
    }
    
    // Start the game as singler player

    public void SinglePlayer ()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        GameManager.instance.StartOnePlayer();
        uI.SetActive(true);
    }

    // Start the game as multiplayer player
    public void MultiPlayer ()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Bring the options menu out
    public void Options ()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ExitOptions ()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ChangeVol(float volume) 
    {
        float newVol = AudioListener.volume;
        newVol = volume;
        AudioListener.volume = newVol;
    }

    public void DayMap ()
    {
        mapGener.MapOfTheDay = !mapGener.MapOfTheDay;
    }

    public void Continue ()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        gameOver.SetActive(false);
        uI.SetActive(false);
    }
}
