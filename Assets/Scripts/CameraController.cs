using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    public InputController playerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (playerController != null)
        {
            player = playerController.tankPlayer;  
        }
        else
        {
            Debug.Log("Theres no player controller");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.instance.gameStart)
        {
            if (player == null)
            {
                Debug.Log("Theres no camera target");
            }
            else
            {
                transform.position = player.transform.position + offset;
            }   
        }     
    }

    public void GetCameraFromController (GameObject tankToFollow)
    {
        player = tankToFollow;
    }
}
