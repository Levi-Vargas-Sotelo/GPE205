using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject Player;
    
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
        //SpawnTank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTank ()
    {
        // Spawn player
        GameObject player = Instantiate (Player,transform.position,Quaternion.identity) as GameObject;
        gameManager.LookforPlayer();
    }
}
