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

    public float musicVolume;
    public float sFXVolume;

    public AudioClip menu;
    public AudioClip game;
    public AudioClip confirm; 

    public AudioSource gameSounds;

    // Get the mape generator object
    public MapGenerator mapGener;

    // Start is called before the first frame update
    void Start()
    {
        mapGener = GameManager.instance.mapGen;
        gameSounds.clip = menu;
        gameSounds.Play();
    }

    // Update is called once per frame
    void Update()
    {
        livesA.text = "Lives: " + GameManager.instance.lives.ToString();
        scoreA.text = "Score: " + GameManager.instance.playerScore.ToString();

        gameSounds.volume = musicVolume;
    }
    
    // Start the game as singler player

    public void SinglePlayer ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        GameManager.instance.StartOnePlayer();
        uI.SetActive(true);
        gameSounds.clip = game;
        gameSounds.Play();
    }

    // Start the game as multiplayer player
    public void MultiPlayer ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Bring the options menu out
    public void Options ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ExitOptions ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ChangeVol(float volume) 
    {
        musicVolume = volume;
    }

    public void ChangeSFX(float SFX)
    {
        sFXVolume = SFX;
    }

    public void DayMap ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);
        mapGener.MapOfTheDay = !mapGener.MapOfTheDay;
    }

    public void Continue ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        gameOver.SetActive(false);
        uI.SetActive(false);
    }
}
