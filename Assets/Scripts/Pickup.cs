using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // The powerup this object holds and adds
    public Powerup powerup;

    // Audio to play whenever the powerup is picked up by anyone
    public AudioClip feedback;

    public Transform tf;

    void Awake ()
    {
        GameManager.instance.powerups.Add(this.gameObject);    
    }

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter (Collider other)
    {
        // We store if the other object we collided has a powerup controller script in a new variable
        PowerupController powCon = other.GetComponent<PowerupController> ();

        // If there is in fact a powerup controller that it detected on the other object
        if (powCon != null)
        {
            // It adds the powerup into the tank
            powCon.Add (powerup);

            // Play the sound clip if there is one attached, if not then it will not play anything
            if (feedback != null) 
            {
                AudioSource.PlayClipAtPoint(feedback, tf.position, 5.0f);
            }

            // After adding the object is destroyed
            Destroy (gameObject);
        }
    }

    void OnDestroy()
    {
        GameManager.instance.powerups.Remove(this.gameObject);
    }
}
