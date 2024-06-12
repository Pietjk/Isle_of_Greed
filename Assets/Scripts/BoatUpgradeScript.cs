using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatUpgradeScript : MonoBehaviour
{
    //variables for boost function
    [SerializeField] private float initialBoostTime = 5;
    private float boostTime;
    [SerializeField] private float initialBoostCooldown = 2;
    private float boostCooldown = 2;
    private bool canBoost = false;
    private bool cooldownCountingDown = false;

    //Variable for teleport function
    [SerializeField] private GameObject ship;

    //Get info form boat class
    [SerializeField] private BoatController boatcontroller;

    private void Start()
    {
        //Sets variables to the variables assigned in the unity editor
        boatcontroller.currentSpeed = boatcontroller.initialSpeed;
        boostTime = initialBoostTime;
        boostCooldown = initialBoostCooldown;
    }

    void Update()
    {
        Boost();
        Teleport();
    }

    /// <summary>
    /// Increases the speed of the boat
    /// </summary>
    public void Boost()
    {
        //Checks if there is enough charge to boost
        if (boostTime > 0)
        {
            canBoost = true;
        }
        //Checks if boost is gone and start the recharge
        else
        {
            canBoost = false;
            boostCooldown -= Time.deltaTime;
        }

        //When ther is enogh charge and the button is pressed charge goes down and speed goes up
        if (Input.GetKey(KeyCode.LeftShift) && canBoost == true)
        {
            boostTime -= Time.deltaTime;
            boatcontroller.currentSpeed = boatcontroller.newSpeed;
        }
        //When charge is gone speed goes back down
        else
        {
            boatcontroller.currentSpeed = boatcontroller.initialSpeed;
        }

        //Checks if the recharge is done and resets evrything
        if (boostCooldown <= 0)
        {
            boostTime = initialBoostTime;
            boostCooldown = initialBoostCooldown;
            canBoost = true;
        }
    }

    /// <summary>
    /// Teleports boat back to hub
    /// </summary>
    public void Teleport()
    {
        //Checks if the 'H' button is pressed and the teleports boat to the coordinates
        if (Input.GetKeyDown(KeyCode.H))
        {
            ship.transform.position = new Vector3(0, 0.36f, 0);
        }
    }
}
