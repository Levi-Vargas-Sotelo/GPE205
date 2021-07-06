using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Select the prefab we want to spawn every amount of time
    public GameObject pickupPrefab;

    // Variable to store the spawned pickup to know if we have a spawned one already or not
    private GameObject spawnedPickup;

    // Variables to know the time for the pickup to spawn
    public float spawnDelay;
    private float nextSpawnTime;

    // Reference of the transform of the spawner to know where to spawn the pickup
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no pickup at the moment 
        if (spawnedPickup == null) 
        {
            // If the timer runs out then spawn a new pickup
            if (Time.time > nextSpawnTime)
            {
                // Instantiate at the same position of the spawner
                spawnedPickup = Instantiate(pickupPrefab,tf.position,Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;
            }
        } 
        else 
        {
            // If there is already a pickup then just keep delaying the timer indefinetely until this changes
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
