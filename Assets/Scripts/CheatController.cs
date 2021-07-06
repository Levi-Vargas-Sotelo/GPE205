using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    public PowerupController powCon;
    public Powerup cheatPowerup;

    // Start is called before the first frame update
    void Start()
    {
        powCon = gameObject.GetComponent<PowerupController> ();
    }

    // Update is called once per frame
    void Update()
    {
        // If the secret code is pressed
        if (Input.GetKey (KeyCode.L)) 
        {
            // Add a powerup to the player
            powCon.Add(cheatPowerup);
        }
    }
}
