using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    // Movement speed of the tank
    public float moveSpeed = 3; 

    // Turning speed of the tank
    public float turnSpeed = 180;

    // Health of player
    public float Health = 5;

    //Shooting delay
    public float shootDelay = 1.0f;

    // Strength of bullet
    public float shootForce = 5;

    // Damage of the bullets
    public float bulletDamage = 2;

    // Current Score of the tank
    public float Score = 0;

    // Points this tank will give
    public float pointsToGive = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
