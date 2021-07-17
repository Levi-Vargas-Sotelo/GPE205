using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Create a enumeration variable to hold all the possible controls for the game
    public enum InputScheme {WASD, arrowKeys};
    
    // Have a variable to see in the inspector to use the enumerator and use that control scheme
    public InputScheme input = InputScheme.WASD;

    // Reference of the tank player object
    public GameObject tankPlayer;
    
    // The player's lives
    public float playerLives;

    // Reference for the camera
    public CameraController playerCamera;
    public GameObject cameraObject;

    // Variable for the score
    public float playerScore;
    public float temporaryScore;

    // Variable for the tank motor script
    public TankMotor motor;

    // Variable for the tank data script
    public TankData data;

    // Var to check if we can shoot
    public bool canShoot = true;
    //private float for delay time
    public float delay;
    // float to know next time we can shoot
    private float nextEventTime;

    public bool gameOver;

    public bool SecondPlayer;

    public Camera playerTankCameraComponent;

    // Awake is called when the GameObject is initialized 
    void Awake()
    {
        
    }

    void Start()
    {
        if (SecondPlayer)
        {
            input = InputScheme.arrowKeys;
        }

        // stating when we can shoot again
        nextEventTime = Time.time + delay;

        GameManager.instance.playersList.Add(this);

        SpawnPlayerCamera (this);
    }

    public void GetPlayerTank (GameObject thisPlayer)
    {
        //tankPlayer = thisPlayer;
        motor = thisPlayer.GetComponent<TankMotor>();
        data = thisPlayer.GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameStart)
        {
            if (tankPlayer == null)
            {
                if (playerLives > 0)
                {
                    playerLives -= 1;
                    SpawnPlayerTank(this);
                    
                    AddPlayerScore ();

                    GameManager.instance.CheckPlayerLives();
                } 
                else if (playerLives <= 0)
                {
                    Debug.Log("Player dead");
                    AddPlayerScore ();
                    gameOver = true;
                }
            }
            else
            {
                temporaryScore = data.Score;
            }

            // Fill the correct UI texts
            if (SecondPlayer == false)
            {
                GameManager.instance.menus.livesA.text = "Lives: " + playerLives.ToString();
            }

            if (SecondPlayer == false)
            {
                GameManager.instance.menus.scoreA.text = "Score: " + temporaryScore.ToString();
            }        
        }

        if(GameManager.instance.gameOver)
        {
            GameManager.instance.menus.finalScoreA.text = "Final Score: " + playerScore;

            if(SecondPlayer == true)
            {
                GameManager.instance.menus.finalScoreB.text = "Final Score: " + playerScore;
            }
        }

        if (GameManager.instance.gameStart)
        {
            // Check what the input is set to
            switch (input) 
            {
                // If the input is set to arrow keys then do this code
                case InputScheme.arrowKeys:
                    // All of these are for moving the tank according to the direction and using the values from the tank data
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        motor.Move(data.moveSpeed);
                    }
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        motor.Move(-data.moveSpeed);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        motor.Rotate(data.turnSpeed);
                    }
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        motor.Rotate(-data.turnSpeed);
                    }
                break;

                // If the input is set to WASD keys then do this code
                case InputScheme.WASD:
                    // All of these are for moving the tank according to the direction and using the values from the tank data
                    if (Input.GetKey(KeyCode.W))
                    {
                        motor.Move(data.moveSpeed);
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        motor.Move(-data.moveSpeed);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        motor.Rotate(data.turnSpeed);
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        motor.Rotate(-data.turnSpeed);
                    }
                break;
            }

            // Shooting input
            if (Input.GetKey(KeyCode.Space))
            {
                // if we can shoot...
                if (canShoot) 
                {
                    // we shoot
                    motor.Shoot(data.shootForce);
                    // and we cant shoot anymore
                    canShoot = false;
                }
            }

            // shoot delay
            delay -= Time.deltaTime;
            // if the delay is over
            if (delay <= 0) 
            {
                // we can shoot again
                canShoot = true;
                // set delay back to the data
                delay = data.shootDelay;
            }
        }
    }

    public void SpawnPlayerTank (InputController theController) 
    {
        // Sends this controller to the manager who then selects a random spawner and sends the same controller to it and then spawns a tank who then sends the player tank back to this objecrt :)
        GameManager.instance.SpawnTank (theController);
        data = tankPlayer.gameObject.GetComponent<TankData>();
        motor = tankPlayer.gameObject.GetComponent<TankMotor>();
        delay = data.shootDelay;
        motor.bStrenght = data.shootForce;
        motor.bDamage = data.bulletDamage;

    }

    public void ThisIsSecondPlayer ()
    {
        SecondPlayer = true;
        GameManager.instance.menus.livesB.text = "Lives: " + playerLives.ToString();
        GameManager.instance.menus.scoreB.text = "Score: " + temporaryScore.ToString();
    }

    public void SpawnPlayerCamera (InputController thisController)
    {
        // Spawns a camera object and then grabs the tank and inserts it into the camera controller script
        cameraObject = Instantiate (cameraObject) as GameObject;
        CameraController camCont = cameraObject.GetComponent<CameraController>();
        playerTankCameraComponent = cameraObject.GetComponent<Camera>();
        camCont.playerController = thisController;
        
        //camCont.player = tankPlayer;
        playerCamera = camCont;
    }

    void AddPlayerScore ()
    {
        playerScore = playerScore + temporaryScore;
    }

    void OnDestroy()
    {
        GameManager.instance.playersList.Remove(this);
    }

    public void AdjustCameraDown ()
    {
        playerTankCameraComponent.rect = new Rect(0, 0, 1.0f, 0.5f);
    }

    public void AdjustCameraUp ()
    {
        playerTankCameraComponent.rect = new Rect(0, 0.5f, 1.0f, 0.5f);
    }
}
