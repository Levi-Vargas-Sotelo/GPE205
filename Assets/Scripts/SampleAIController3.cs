using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController3 : MonoBehaviour
{
    // Variables to reference the motor and data of the tank
    private TankData data;
    private TankMotor motor;
    // Variable for the transform of the tank
    private Transform tf;
    // Variable for the target object the tank wants to reach
    public Transform target;

    // What stage of the obstacle avoidance system the tank is in
    private int avoidanceStage = 0;
    // Time the tank takes after avoiding something
    public float avoidanceTime = 2.0f;
    // Variable to reference the time for avoidance
    private float exitTime;
    
    // Enumerator for the behaviour of the tank
    public enum AttackMode { Chase };
    // Variable to choose the behaviour we want
    public AttackMode attackMode;

    // Awake is called when the GameObject is initialized 
    void Awake()
    {
        // Assign both variables the respective scripts they should reference
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If we are chasing
        if (attackMode == AttackMode.Chase) 
        {
            // If we are avoiding:
            if (avoidanceStage != 0) 
            {
                // Then avoid
                DoAvoidance();
            }
            // If we are not
            else 
            {
                // Keep chasing target
                DoChase();
            }
        }
    }

    void DoChase () 
    {
        // Spin towards the target
        motor.RotateTowards(target.position, data.turnSpeed);
        // See if we can move to this object using the tank's normal speed
        if (CanMove (data.moveSpeed)) 
        {
            // If it can then move to it
            motor.Move (data.moveSpeed);
        } else 
        {
            // If it can not, then start avoiding the obstacle
            avoidanceStage = 1;
        }
    }

    void DoAvoidance () 
    {
        // If we are avoiding
        if (avoidanceStage == 1) 
        {
            // Spin to the left since it is negative
            motor.Rotate(-1 * data.turnSpeed);

            // If moving forward is possible
            if (CanMove (data.moveSpeed) ) 
            {
                // Move to the next stage of avoiding
                avoidanceStage = 2;
                // Make the variable reference the one set by designers
                exitTime = avoidanceTime;
            }

        // If we are in second stage
        } else if (avoidanceStage == 2) 
        {
            // If we can advance using the tank's speed
            if (CanMove(data.moveSpeed)) 
            {
                // Wait for the seconds we set
                exitTime -= Time.deltaTime;
                
                // Move forward
                motor.Move(data.moveSpeed);

                // If the time has passed
                if (exitTime <= 0) 
                {
                    // Return to chasing the target
                    avoidanceStage = 0;
                }                     
            } else 
            {
                // If we still can not move then do the avoiding all over again
                avoidanceStage = 1;
            }             
        }
    }

    bool CanMove ( float speed ) 
    {
        // Shoot a raycast in drection of
        RaycastHit hit;

        // Check if the raycast shot hit any object
        if (Physics.Raycast (tf.position, tf.forward, out hit, speed)) 
        {
            // Check if it is not the player by using the tags system and assigning the player the "Player" tag
            if (!hit.collider.CompareTag("Player")) 
            {
                // ... then we can't move
                return false;
            }
        }

    // Since there is no obstacle, then keep moving to the target
    return true;
    }
}
