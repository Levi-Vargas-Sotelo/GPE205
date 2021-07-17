using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Text livesB;
    public Text scoreB;

    public Text finalScoreA;
    public Text finalScoreB;

    public float musicVolume;
    public float sFXVolume;

    public AudioClip menu;
    public AudioClip game;
    public AudioClip confirm; 
    public AudioClip death;

    public AudioSource gameSounds;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
        
    }

    // Update is called once per frame
    void Update()
    {
        //livesA.text = "Lives: " + GameManager.instance.lives.ToString();
        //scoreA.text = "Score: " + GameManager.instance.playerScore.ToString();

        gameSounds.volume = musicVolume;
    }
    
    // Start the game as singler player

    public void MainMenu()
    {
        gameSounds.clip = menu;
        gameSounds.Play();
        finalScoreA.text = "";
        finalScoreB.text = "";
    }

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
        GameManager.instance.StartTwoPlayer();
        uI.SetActive(true);
        gameSounds.clip = game;
        gameSounds.Play();
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
        MapGenerator.mapGen.MapOfTheDay = !MapGenerator.mapGen.MapOfTheDay;
    }

    public void Continue ()
    {
        AudioSource.PlayClipAtPoint(confirm, this.transform.position, GameManager.instance.sFX);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GameDone ()
    {
        uI.SetActive(false);
        gameOver.SetActive(true);
        gameSounds.Pause();
        AudioSource.PlayClipAtPoint(death, this.transform.position, GameManager.instance.music);
    }
}
