using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocatorScript : MonoBehaviour
{
    //Itemdetector variables
    [SerializeField] private float itemDistanceFromplayer;
    [SerializeField] private GameObject player;
    private LayerMask mask;

    //Timer variables
    [SerializeField] private float initialLocaterOnTime = 2f;
    private float locaterOnTime;
    [SerializeField] private float initialLocaterCooldownTimer = 5f;
    private float locaterCooldownTimer;
    private bool canLocate = false;
    private bool countingDown = false;

    //Arrowpointer variables
    [SerializeField] private float rotationSpeed = 5f;
    private Vector3 item;
    private Vector3 targetDirection;
    private Renderer renderer;

    void Start()
    {
        //Looks for meshrenderer of arrowmodel and turns renderer off
        renderer = transform.Find("arrowModel").GetComponent<MeshRenderer>();
        renderer.enabled = false;

        //Looks foor all objects in the scene with the layer item
        mask = LayerMask.GetMask("Item");

        //Sets variables to the variables assigned in the unity editor
        locaterOnTime = initialLocaterOnTime;
        locaterCooldownTimer = initialLocaterCooldownTimer;
    }
    
    void Update()
    {
        //Points arrow model in the direction of the nearest item
        targetDirection = item - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f));
        ItemLocater();
    }

    /// <summary>
    /// Locates the closest item in relation to the player
    /// </summary>
    public void ItemLocater()
    {
        //Put all items in a certain radius in an array
        Collider[] items = Physics.OverlapSphere(player.transform.position, itemDistanceFromplayer, mask);

        //Loop through all items in the array
        for (int i = 0; i < items.Length; i++)
        {
            //Select the item with the shortest distance from the player
            if (Vector3.Distance(items[i].transform.position, transform.position) < Vector3.Distance(item, transform.position))
            {
                item = items[i].transform.position;
            }
        }
        LocaterTimers();
    }

    /// <summary>
    /// The timers for the ItemLocater
    /// </summary>
    private void LocaterTimers()
    {
        //Checks wheter or not the item locater can turn on
        if (locaterOnTime > 0)
        {
            canLocate = true;
        }
        else
        {
            canLocate = false;
            renderer.enabled = false;
        }

        //Turns the locator renderer on if conditions are met and the button is pressed. 
        //Also starts counting down
        if (Input.GetKey(KeyCode.R) && canLocate == true)
        {
            countingDown = true;
            renderer.enabled = true;
        }

        //Starts counting down
        if (countingDown == true)
        {
            locaterOnTime -= Time.deltaTime;
        }

        //When the ontime timer has reached zero starts the cooldown timer
        if (locaterOnTime <= 0)
        {
            countingDown = false;
            locaterCooldownTimer -= Time.deltaTime;
        }

        //If the cooldown reches zero resets all the variables
        if (locaterCooldownTimer <= 0)
        {
            locaterOnTime = initialLocaterOnTime;
            locaterCooldownTimer = initialLocaterCooldownTimer;
            canLocate = true;
        }
    }
}
