using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    // Make a list of all our powerups which will increase as we make more
    public List<Powerup> powerups;

    // Variable for using the tank's data
    private TankData data;

    void Awake()
    {
        // Reference the data of the tank in this variable
        data = gameObject.GetComponent<TankData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize list when the game starts
        powerups = new List<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        // Create the list we made for the powerups that have run out of time
        List<Powerup> expiredPowerups = new List<Powerup>();

        // For each loop function that goes through each of the powerups in the list
        foreach (Powerup power in powerups) 
        {
            // For each powerup, we subtract from the timer it has
            power.duration -= Time.deltaTime;

            // If the time of the powerup has run out then add it to the expired list ones
            if (power.duration <= 0) 
            {
                expiredPowerups.Add (power);
            }
        }

        // After looking on the powerups list we now look through the expired ones list
        foreach (Powerup power in expiredPowerups)
        {
            // For each loop we use the deactivate function on the powerup to remove its effects on the tank as well as then removing it from the active powerups list
            power.OnDeactivate(data);
            powerups.Remove(power);
        }
        
        // After the expired ones loop is done, it is cleared completely using this function
        expiredPowerups.Clear ();
    }

    // Function to add a powerup to the for each loop list
    public void Add (Powerup powerup)
    {
        // Use the onActivate function of the powerup on the tank to start
        powerup.OnActivate (data);
        
        // We check if the powerup is permanent, if it is, then do not add it to the list as there is no need to keep track of it anymore
        if (!powerup.isPermanent) 
        {
        powerups.Add (powerup);
        }
    }
}
