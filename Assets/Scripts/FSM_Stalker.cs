using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Stalker : MonoBehaviour
{
    // AI states and variable to reference them
    public enum AIState {Pursue, Retreat, Heal};
    public AIState aiState = AIState.Pursue;

    public float aiSenseRadius;

    // Variables to reference elements of the tank to use and the target as well
    private TankData data;
    private TankMotor motor;
    public Transform target;
    private Transform tf;

    // To know what step of avoiding the tank is in
    private int avoidanceStage = 0;

    // Time the tank takes after avoiding something
    public float avoidanceTime = 2.0f;
    // Variable to use the value for avoiding something
    private float exitTime;

    // Value for the time the tank goes into fleeing
    public float fleeTime = 10;
    private float lastFleeTime; 
    
    // Variable to know last time the tank shot to make a firing rate for it
    private float lastShootTime;

    // Rate for healing the tank while resting
    public float HealingTime;

    // How much the tank flees
    public float fleeDistance = 1.0f; 

    // Variable to know distance between player in a float format rather than vector 3
    private float playerDistance;

    public DetectPlayer sensePlayer;

    void Awake()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
        motor.bStrenght = data.shootForce;
        motor.bDamage = data.bulletDamage;  

        GameManager.instance.enemies.Add(this.gameObject);
    }

    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = sensePlayer.playerTank;

        
        
        switch (aiState)
        {
            case AIState.Pursue:
                if (target != null)
                {
                    CheckPlayerDistance ();

                    // If we are avoiding
                    if (avoidanceStage != 0) 
                    {
                        DoAvoidance();
                    } else 
                    {
                        DoChase();

                        // Shoot according to the firing rate we set
                        if (Time.time > lastShootTime + data.fireRate) 
                        {
                            motor.Shoot(data.shootForce);
                            lastShootTime = Time.time;
                        }
                    }

                    // If tank is damaged
                    if (data.health < data.maxHealth) 
                    {   
                        // Retreat
                        ChangeState(AIState.Retreat);
                    } 
                    // If the player is far away from the sense radius
                    else if (playerDistance <= aiSenseRadius) 
                    {
                        ChangeState(AIState.Pursue);
                    }
                }
            break;
            
            case AIState.Retreat:
                if (target != null)
                {
                    // Start fleeing timer
                    lastFleeTime -= Time.deltaTime;

                    // Flee and avoid
                    if (avoidanceStage != 0) 
                    {
                        DoAvoidance();
                    } else 
                    {
                        Flee();
                    }

                    if (lastFleeTime <= 0)
                    {
                        lastFleeTime = fleeTime;
                        ChangeState (AIState.Heal);
                    }
                }
            break;

            case AIState.Heal:
                // Perform Behaviors
                DoRest();
                
                if (target != null)
                {
                    // See if the player is far enough so tank can heal
                    if (Vector3.Distance (target.position, tf.position) <= aiSenseRadius) 
                    {
                        ChangeState(AIState.Retreat);
                    }
                    // If health was regained, then chase again
                    else if (data.health >= data.maxHealth) 
                    {
                        ChangeState(AIState.Pursue);
                    }
                }
            break;
        }
    }

    void DoChase () 
    {
        if (target != null) 
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

    public void DoRest() 
    {
        // Increase our health. Remember that our increase is "per second"!
        data.health += HealingTime * Time.deltaTime;

        // But never go over our max health
        data.health = Mathf.Min (data.health, data.maxHealth);
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

    void Flee () 
    {
        if (target != null)
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
            motor.RotateTowards( fleePosition, data.turnSpeed );
            motor.Move (data.moveSpeed);

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
    }

    void CheckPlayerDistance ()
    {
        if (target != null)
        {
            float distanceFromPlayer = Vector3.Distance (tf.transform.position, target.transform.position);
            playerDistance = distanceFromPlayer;
        }
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
