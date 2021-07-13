using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Patrol : MonoBehaviour
{
    // AI states and variable to reference them
    public enum AIState {Patrol, Attack, Check};
    public AIState aiState = AIState.Patrol;

    // Array for the waypoints the tank will go through
    public Transform[] waypoints;

    // Variable to reference which waypoint we are in right now
    private int currentWaypoint = 0;

    // Variable for how close we want the tank to reach each point
    public float closeEnough = 1.0f;

    // Variables to reference elements of the tank to use and the target as well
    private TankData data;
    private TankMotor motor;
    public Transform target;
    private Transform tf;

    // Variable to know last time the tank shot to make a firing rate for it
    private float lastShootTime;

    // Distance in which the tank will be aware of the player
    public float aiSenseRadius;

    // Value for the time the tank has to check for the player
    public float checkTime = 4;
    private float lastCheckTime;

    // Variable to know distance between player in a float format rather than vector 3
    private float playerDistance;

    // Prefab for the waypoint to spawn
    public GameObject Waypoints;

    // Awake is called when the GameObject is initialized
    void Awake()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
        motor.bStrenght = data.shootForce;
        motor.bDamage = data.bulletDamage;

        GameManager.instance.enemies.Add(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.instance.player.transform;

        Waypoints.transform.parent = null;        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = GameManager.instance.player.transform;
        }

        switch (aiState)
        {
            case AIState.Patrol:
                Patrol ();
                CheckPlayerDistance ();
                lastCheckTime = checkTime;

                if (playerDistance <= aiSenseRadius)
                {
                    lastShootTime = data.fireRate;
                    ChangeState (AIState.Attack);
                }
            break;

            case AIState.Attack:
                Chase ();
                CheckPlayerDistance ();

                lastShootTime -= Time.deltaTime;

                // Limit our firing rate, so we can only shoot if enough time has passed
                if (Time.time > lastShootTime + data.fireRate) 
                {
                    motor.Shoot(data.shootForce);
                    lastShootTime = Time.time;
                }

                if (playerDistance >= aiSenseRadius)
                {
                    ChangeState (AIState.Check);
                }

            break;

            case AIState.Check:
                CheckPlayerDistance ();
                lastCheckTime -= Time.deltaTime;

                if (lastCheckTime <= 0)
                {
                    ChangeState (AIState.Patrol);
                }
                
                if (playerDistance <= aiSenseRadius)
                {
                    lastShootTime = data.fireRate;
                    ChangeState (AIState.Attack);
                }
            break;
        }
    }

    void Patrol ()
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
            } else
            {
                // If it is the last, then set it all over again
                currentWaypoint = 0;
            }
        }
    }

    void Chase () 
    {
        // Spin towards the target
        motor.RotateTowards(target.position, data.turnSpeed);
        // If it can then move to it
        motor.Move (data.moveSpeed);
    }

    // Check to see the distance from the player to the tank and make it a float
    void CheckPlayerDistance ()
    {
        float distanceFromPlayer = Vector3.Distance (tf.transform.position, target.transform.position);
        playerDistance = distanceFromPlayer;
    }

    // Change to a new state using this function
    public void ChangeState ( AIState newState ) 
    {
        // Change our state
        aiState = newState;
    }

    void OnDestroy()
    {
        GameManager.instance.enemies.Remove(this.gameObject);
    }
}
