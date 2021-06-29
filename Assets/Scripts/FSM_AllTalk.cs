using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_AllTalk : MonoBehaviour
{
    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;
    public float stateEnterTime;
    public float aiSenseRadius;
    public float restingHealRate;
    private TankData data;
    private TankMotor motor;
    public Transform target;
    private Transform tf;
    private int avoidanceStage = 0;
    // Time the tank takes after avoiding something
    public float avoidanceTime = 2.0f;
    private float exitTime;
    private float lastShootTime = 1;
    public float fleeDistance = 1.0f; 


    void Awake()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
        motor.bStrenght = data.shootForce;
        motor.bDamage = data.bulletDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () 
    {
        if ( aiState == AIState.Chase ) 
        {
            // Perform Behaviors
            if (avoidanceStage != 0) 
            {
                DoAvoidance();
            } else 
            {
                DoChase();
            }

            // If health is low
            if (data.health < data.maxHealth * 0.5f) 
            {
                ChangeState(AIState.CheckForFlee);
            } else if (Vector3.Distance (target.position, tf.position) <= aiSenseRadius) 
            {
                ChangeState(AIState.ChaseAndFire);
            }
        } else if ( aiState == AIState.ChaseAndFire ) 
        {
            // Perform Behaviors
            if (avoidanceStage != 0) 
            {
                DoAvoidance();
            } else 
            {
                DoChase();

                // Limit our firing rate, so we can only shoot if enough time has passed
                if (Time.time > lastShootTime + data.fireRate) 
                {
                    motor.Shoot(data.shootForce); // Note: This assumes we have a "shooter" component with a "Shoot()" function
                    lastShootTime = Time.time;
                }
            }
            // Check for Transitions
            if (data.health < data.maxHealth * 0.5f) 
            {
                ChangeState(AIState.CheckForFlee);
            } else if (Vector3.Distance (target.position, tf.position) > aiSenseRadius) 
            {
                ChangeState(AIState.Chase);
            }
            } else if ( aiState == AIState.Flee ) 
            {
                // Perform Behaviors
                if (avoidanceStage != 0) 
                {
                    DoAvoidance();
                } else 
                {
                    DoFlee();
                }

            // Check for Transitions
            if (Time.time >= stateEnterTime + 30) 
            {
                ChangeState(AIState.CheckForFlee);
            }
        } else if ( aiState == AIState.CheckForFlee ) 
        {
            // Perform Behaviors
            CheckForFlee();

            // Check for Transitions
            if (Vector3.Distance (target.position, tf.position) <= aiSenseRadius) 
            {
                ChangeState(AIState.Flee);
            } else 
            {
                ChangeState(AIState.Rest);
            }
        } else if ( aiState == AIState.Rest ) 
        {
            // Perform Behaviors
            DoRest();

            // Check for Transitions
            if (Vector3.Distance (target.position, tf.position) <= aiSenseRadius) 
            {
                ChangeState(AIState.Flee);
            } else if (data.health >= data.maxHealth) 
            {
                ChangeState(AIState.Chase);
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

    public void CheckForFlee() 
    {
        // TODO: Write the CheckForFlee state.
    }

    public void DoRest() 
    {
        // Increase our health. Remember that our increase is "per second"!
        data.health += restingHealRate * Time.deltaTime;

        // But never go over our max health
        data.health = Mathf.Min (data.health, data.maxHealth);
    }

    void DoFlee () 
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
    }

    public void ChangeState ( AIState newState ) 
    {
        // Change our state
        aiState = newState;

        // save the time we changed states
        stateEnterTime = Time.time;
    }
}
