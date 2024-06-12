using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpgradeScript : MonoBehaviour
{
    //variables for the hight a player can jump
    private bool canJumpHigh = false;
    private float newHight;

    //Get info form character class
    [SerializeField] private CharacterController characterController;

    private void Start()
    {
        //calculate the new jump height
        newHight = characterController.initialJumpHeight * 3;
    }

    void Update()
    {
        HighJump();
    }

    public void HighJump()
    {
        // the c button is pressed make the jumpheight the new jumpheight
        if (Input.GetKey(KeyCode.C))
        {
            characterController.jumpHeight = newHight;
        }
        else
        //set height back to normal
        {
            characterController.jumpHeight = characterController.initialJumpHeight;
        }

    }
}
