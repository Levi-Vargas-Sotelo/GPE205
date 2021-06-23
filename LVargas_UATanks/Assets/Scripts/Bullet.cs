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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletForce;

        Destroy(gameObject, destroyTime);
    }

    // Destroy enemy and possibly make it take damage in the future through this using the bullet damage
    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Tank
        if (other.tag == "Tank")
        {
            //Destroy it
            Debug.Log("die");
            Destroy(other.gameObject);
        }
    }
}
