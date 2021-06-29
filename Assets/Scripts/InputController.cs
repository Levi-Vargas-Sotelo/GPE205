using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Create a enumeration variable to hold all the possible controls for the game
    public enum InputScheme {WASD, arrowKeys};
    
    // Have a variable to see in the inspector to use the enumerator and use that control scheme
    public InputScheme input = InputScheme.WASD;

    // Variable for the tank motor script
    private TankMotor motor;

    // Variable for the tank data script
    private TankData data;

    // Var to check if we can shoot
    public bool canShoot = true;
    //private float for delay time
    public float delay;
    // float to know next time we can shoot
    private float nextEventTime;

    // Awake is called when the GameObject is initialized 
    void Awake()
    {
        // Assign both variables the respective scripts they should reference
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        delay = data.shootDelay;
    }

    void Start()
    {
        // stating when we can shoot again
        nextEventTime = Time.time + delay;

        motor.bStrenght = data.shootForce;
        motor.bDamage = data.bulletDamage;
    }

    // Update is called once per frame
    void Update()
    {
        // Check what the input is set to
        switch (input) 
        {
            // If the input is set to arrow keys then do this code
            case InputScheme.arrowKeys:
                // All of these are for moving the tank according to the direction and using the values from the tank data
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    motor.Rotate(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    motor.Rotate(-data.turnSpeed);
                }
            break;

            // If the input is set to WASD keys then do this code
            case InputScheme.WASD:
                // All of these are for moving the tank according to the direction and using the values from the tank data
                if (Input.GetKey(KeyCode.W))
                {
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    motor.Rotate(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    motor.Rotate(-data.turnSpeed);
                }
            break;
        }

        // Shooting input
        if (Input.GetKey(KeyCode.Space))
        {
            // if we can shoot...
            if (canShoot) 
            {
                // we shoot
                motor.Shoot(data.shootForce);
                // and we cant shoot anymore
                canShoot = false;
            }
        }

        // shoot delay
        delay -= Time.deltaTime;
        // if the delay is over
        if (delay <= 0) 
        {
            // we can shoot again
            canShoot = true;
            // set delay back to the data
            delay = data.shootDelay;
        }
    }
}
