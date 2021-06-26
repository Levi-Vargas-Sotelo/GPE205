using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController : MonoBehaviour
{
    public Transform[] waypoints;

    // References to both the data and motor
    public TankMotor motor;
    public TankData data;

    // Variable to reference which waypoint we are in right now
    private int currentWaypoint = 0;

    // Variable for how close we want the tank to reach each point
    public float closeEnough = 1.0f;

    // Variable to reference the transform of this tank
    public Transform tf;

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
        // checking if the tank is still not looking at the waypoint
        if (motor.RotateTowards( waypoints[currentWaypoint].position, data.turnSpeed ) ) 
        {
            // Left blank for possible future use
        }
        // If we are not spinning anymore because we are looking at it
        else 
        {
            // Advance to the waypoint
            motor.Move(data.moveSpeed);
        }
        // If the tank is on the distance we set to be close enough by using quaternions
        if (Vector3.SqrMagnitude (waypoints[currentWaypoint].position - tf.position) < (closeEnough * closeEnough)) 
        {
            // Move to the next waypoint in the array
            if (currentWaypoint < waypoints.Length-1) 
            {
                currentWaypoint++;
            }    
        }
    }
}
