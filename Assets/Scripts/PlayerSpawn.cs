using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject Player;

    void Awake ()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.spawners.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTank (InputController playerControllerWhoSpawnedTank)
    {
        // Spawn player
        GameObject player = Instantiate (Player,transform.position,Quaternion.identity) as GameObject;
        //GameManager.instance.LookforPlayer();
        playerControllerWhoSpawnedTank.tankPlayer = player;
    }

    void OnDestroy()
    {
        GameManager.instance.spawners.Remove(this.gameObject);
    }

}
