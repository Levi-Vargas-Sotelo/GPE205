using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Slacker : MonoBehaviour
{
    // AI states and variable to reference them
    public enum AIState {Panic, Flee, Sleep};
    public AIState aiState = AIState.Sleep;

    // Variables to reference elements of the tank to use and the target as well
    private TankData data;
    private TankMotor motor;
    public Transform target;
    private Transform tf;

    // Distance in which the tank will be aware of the player
    public float aiSenseRadius;

    // How much the tank heals while sleeping
    public float healRate;

    // Variable for the distance we want the AI to flee
    public float fleeDistance = 0.5f;

    // Variable to know distance between player in a float format rather than vector 3
    private float playerDistance;

    // Value for the time the tank goes into panic and attacks
    public float panicTime = 7;
    private float lastPanicTime;

    // Value for the time the tank goes into fleeing
    public float fleeTime = 10;
    private float lastFleeTime; 

    // Variable to know last time the tank shot to make a firing rate for it
    private float lastShootTime = 1;

    // Stage of avoidance variable to know if it is dodging
    private int avoidanceStage = 0;
    // Time the tank takes after avoiding something
    public float avoidanceTime = 3.0f;
    private float exitTime;
    
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
        lastPanicTime = panicTime;
        target = GameManager.instance.player.transform;
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
            case AIState.Sleep:
                // Regain health with the value defined multiplied by real time
                data.health += healRate * Time.deltaTime;

                // Prevent it from exceeding the max health value
                data.health = Mathf.Min (data.health, data.maxHealth);

                CheckPlayerDistance ();

                if (playerDistance <= aiSenseRadius)
                {
                    ChangeState (AIState.Panic);
                }
            break;

            case AIState.Panic:
                
                lastPanicTime -= Time.deltaTime;

                if (Time.time > lastShootTime + data.fireRate) 
                {
                    motor.Shoot(data.shootForce);
                    lastShootTime = Time.time;
                }
                
                if (lastPanicTime <= 0)
                {
                    lastPanicTime = panicTime;
                    ChangeState (AIState.Flee);
                }

                motor.RotateTowards(target.position, data.turnSpeed);
            break;

            case AIState.Flee:
                lastFleeTime -= Time.deltaTime;
                Flee ();

                if (lastFleeTime <= 0)
                {
                    lastFleeTime = fleeTime;
                    ChangeState (AIState.Sleep);
                }
            break;
        }
    }

    void Flee ()
    {
        // We subtract the position of the player from the tank position to know the distance between them and store it in a Vector3 variable
        Vector3 vectorToTarget = target.position - tf.position;

        // We multiply that vector by negative 1 to make it the exact opposite 
        Vector3 vectorAwayFromTarget = -1 * vectorToTarget;

        // We normalize the vector which will make it one unit exactly
        vectorAwayFromTarget.Normalize ();

        // Since it is normalized now we can easily mutliply it by any number we want to make it the distance we want it to flee
        vectorAwayFromTarget *= fleeDistance;

        // We find a vector by adding the tank position and the previous vector we got to find a point to flee to
        Vector3 fleePosition = vectorAwayFromTarget + tf.position;

        // We pass that data to the motor and move to that point
        motor.RotateTowards(fleePosition, data.turnSpeed);
        motor.Move (data.moveSpeed);

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