using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    // Variable for the Character Controller
    private CharacterController characterController;

    // Variable for the transform of the tank
    public Transform tf;

    // Point from where to launch bullet
    public GameObject Cannon;
    // Bullet Prefab
    public GameObject Bullet;

    // To reference the damage from the data
    public float bDamage;

    // To reference the strenght from the data
    public float bStrenght;

    
    // Awake is called when the GameObject is initialized 
    public void Awake() 
    {
        // Makes tf reference the transform component to use it instead of getting the component everytime
        tf = gameObject.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Store CharacterController in the variable
        characterController = gameObject.GetComponent<CharacterController> ();     
    }

    // Update is called once per frame
    public void Update()
    {

    }

    // Forward tank movement
    public void Move( float speed )
    {
        // Create a vector for the speed
        Vector3 speedVector;

        // Make the vector have the same direction as the object the script is on and apply the speed value to the it.
        speedVector = tf.forward;
        speedVector *= speed;

        // Use the Simple Move function from the CharacterController
        characterController.SimpleMove (speedVector);
    }

    // Tank rotation
    public void Rotate( float speed ) {
        // Create vector for the rotation info
        Vector3 rotateVector;

        // Rotate vector to the right because it is a positive value and make it use speed value
        rotateVector = Vector3.up;
        rotateVector *= speed;

        // Make the rotate function use real time instead of frames
        rotateVector *= Time.deltaTime;

        // Rotate the tank using local space
        tf.Rotate (rotateVector, Space.Self);
    }

    // Shooting 
    public void Shoot( float delay) 
    {
        //instantiate bullet at cannon position
        GameObject myBullet = Instantiate (Bullet, Cannon.transform.position, Cannon.transform.rotation) as GameObject;
        Bullet bulletData = myBullet.GetComponent<Bullet>();
        // make the bullet reference the tank that shot it
        bulletData.shooter = this.gameObject;
        // make the bullet get the data from the tank
        bulletData.bulletForce = bStrenght;
        bulletData.bulletDamage = bDamage;
    }



    // Function that rotates the tank towards the current waypoint
    public bool RotateTowards ( Vector3 target, float speed )
    {
    // Vector to know distance of target
    Vector3 vectorToTarget;

    // We subtract the position of the waypoint from the tank position to know the distance between them and store it in a Vector3 variable
    vectorToTarget = target - tf.position;

    // Use a quaterion that points towards the target using the vector we got before
    Quaternion targetRotation = Quaternion.LookRotation (vectorToTarget);
         

    // We rotate the tank using the quaternion function between the tank's current rotation, the target rotation vector and with the speed we set on the data
    tf.rotation = Quaternion.RotateTowards (tf.rotation, targetRotation, speed * Time.deltaTime);

    // When the tank gets to that rotation:
    if ( targetRotation == tf.rotation ) 
    {
        
        return false;
    }

    // Finished rotation
    return true;
    }
}
