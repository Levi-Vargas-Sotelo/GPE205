using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //How fast it travels
    public float bulletForce;
    //How much damage it deals
    public float bulletDamage;

    //private float for delay time
    public float destroyTime;

    //references the tank that shot the bullet
    public GameObject shooter; 

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        //moves the bullet at a constant speed and direction
        transform.position += transform.forward * Time.deltaTime * bulletForce;
    }

    // Destroy enemy and possibly make it take damage in the future through this using the bullet damage
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object hit has a tank data to affect
        if (other.GetComponent<TankData>() != null) 
        {
            // If it does, then damage it accordingly and destroy bullet
            TankData otherTank = other.gameObject.GetComponent<TankData>();
            TankData shooterTank = shooter.gameObject.GetComponent<TankData>();
            otherTank.health -= bulletDamage;
            if(otherTank.health <= 0)
            {
                shooterTank.Score += otherTank.pointsToGive;
            }
            Destroy(gameObject);
        }

        // Destroy bullet
        Destroy(gameObject);
    }
}
