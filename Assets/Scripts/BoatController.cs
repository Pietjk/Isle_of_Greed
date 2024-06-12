using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class BoatController : MonoBehaviour
{
    //boat variables
    [SerializeField] public float initialSpeed;
    [SerializeField] public float newSpeed;
    [SerializeField] public float currentSpeed = 3000;
    [SerializeField] private float distanceFromBoat = 20f;

    //turning variables
    [SerializeField] private float angularAccelerationDrag = 0.08f;
    [SerializeField] private float angularDecelerationDrag = 1;
    private float turnSpeed = 10;
    private float maxTurnVelocity = 0.25f;

    [SerializeField] private TMP_Text pressE;

    //gametracker for location of boat
    [SerializeField] private GameObject spawnableGameTracker;
    [SerializeField] private GameObject player;

    //assigned components
    private Rigidbody rb;
    private GameTracker gameTracker;
    [SerializeField] private GameObject compass;
    [SerializeField] private GameObject BoatUI;
    [SerializeField] private GameObject playerUI;

    private void Awake()
    {
        //creates the gametracker
        if (GameObject.Find("GameTracker") == null)
        {
            Instantiate(spawnableGameTracker).name = "GameTracker";
        }
    }

    void Start()
    {
        //Makes sure you have a rigidbody in the scene you can call with "rb"
        //Looks for an object by the name of gametracker 
        //and saves the location of the player in it
        rb = GetComponent<Rigidbody>();
        gameTracker = GameObject.Find("GameTracker").GetComponent<GameTracker>();

        //puts the player at the old height
        transform.position = new Vector3(gameTracker.shipPosition.x, 0.36f, gameTracker.shipPosition.z);

        player.SetActive(false);
        playerUI.SetActive(false);
    }

    void Update()
    {
        PlayerInputs();
        DockDistance();
    }

    /// <summary>
    /// Limits the turnspeed
    /// </summary>
    private void AngularDragClamp()
    {
        //Limit turnspeed to right
        if (rb.angularVelocity.y > maxTurnVelocity)
        {
            rb.angularVelocity = new Vector3(0, maxTurnVelocity, 0);
        }

        //Limit turnspeed to left
        if (rb.angularVelocity.y < -maxTurnVelocity)
        {
            rb.angularVelocity = new Vector3(0, -maxTurnVelocity, 0);
        }
    }

    /// <summary>
    /// Handles player inputs and movement
    /// </summary>
    private void PlayerInputs()
    {
        AngularDragClamp();

        //multiply turnspeed with movementspeed
        turnSpeed = rb.velocity.magnitude * 20;

        //moves forward
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * currentSpeed, ForceMode.Force);
        }
        //move backward
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * currentSpeed * 0.3f, ForceMode.Force);
        }

        //checks wheter or not player is turning
        if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
        {
            //Turn left
            if (Input.GetKey(KeyCode.A))
            {
                rb.angularDrag = angularAccelerationDrag;
                rb.AddTorque(transform.up * turnSpeed * -1);
            }
            //turn right
            if (Input.GetKey(KeyCode.D))
            {
                rb.angularDrag = angularAccelerationDrag;
                rb.AddTorque(transform.up * turnSpeed * 1);
            }
        }
        //Puts the drag to decelerationdrag so the boat slows down fast enough
        else
        {
            rb.angularDrag = angularDecelerationDrag;
        }        
    }

    /// <summary>
    /// Enables you to dock
    /// </summary>
    private void DockDistance()
    {
        //Puts all gameobjects with the layer Interactible in a layermask
        LayerMask mask = LayerMask.GetMask("Interactable");

        //Puts the gameobjects within 5 meters of the player in an array
        Collider[] interactables = Physics.OverlapSphere(transform.position, distanceFromBoat, mask);

        //Empties textfield pressE
        if (interactables.Length <= 0)
        {
            pressE.text = "";
        }

        //goes trough each item in the array
        for (int i = 0; i < interactables.Length; i++)
        {
            //filters out objects with the tag Dock
            if (interactables[i].gameObject.tag == "Dock")
            {
                //Fills textfield pressE
                pressE.text = "[Press E]";

                //Loads scene for character gameplay
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //activates player
                    player.SetActive(true);
                    player.transform.position = interactables[i].transform.position;
                    player.GetComponent<CharacterController>().cameraScript.puzzelGenerator = interactables[i].GetComponent<IslandTracker>().puzzelgenerator;

                    //disables ui
                    pressE.text = "";
                    compass.SetActive(false);
                    BoatUI.SetActive(false);
                    playerUI.SetActive(true);

                    gameTracker.shipPosition = transform.position;

                    //disables boat
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
