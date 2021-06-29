using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController2 : MonoBehaviour
{
    // Variables to reference the motor and data of the tank
    private TankData data;
    private TankMotor motor;
    // Variable for the transform of the tank
    private Transform tf;

    // The target we want the tank to act upon which is the player in this case
    public Transform target;

    // Enumerator to select what we want the AI to do
    public enum AttackMode {Chase, Flee};
    // Variable to select a mode of acting of the AI
    public AttackMode attackMode;

    // Variable for the distance we want the AI to flee
    public float fleeDistance = 1.0f;  

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
        switch (attackMode)
        {
            case AttackMode.Chase:
                if (attackMode == AttackMode.Chase) 
                {
                // Spin tank towards the player
                motor.RotateTowards( target.position, data.turnSpeed );
                // Advance towards the target
                motor.Move (data.moveSpeed);
                }  
            break;
        
            case AttackMode.Flee:
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
            break;
        }
    }
}
