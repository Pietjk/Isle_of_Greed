using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class CharacterController : MonoBehaviour
{
    //configurables
    [SerializeField] private float speed = 1;
    [SerializeField] public float initialJumpHeight = 1;
    public float jumpHeight;
    [SerializeField] private float groundDetectionDistance = 2.59f;
    [SerializeField] private float maxSpeed = 7.5f;

    private float currentPlayerHeight;

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;

    public bool canMove = true;

    private int layerMask = ~(1 << 8);

    //assigned components
    private Rigidbody rb;

    public CameraScript cameraScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPlayerHeight = transform.position.z;
        jumpHeight = initialJumpHeight;
    }
    
    void FixedUpdate()
    {
        ClampVelocity();
        if (canMove)
        {
            PlayerInputs();
        }
    }

    private void ClampVelocity()
    {
        //limits forward velocity
        if (rb.velocity.z > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        }
        //limits right velocity
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
        }
        //limits backward velocity
        if (-rb.velocity.z > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxSpeed);
        }
        //limits left velocity
        if (-rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);
        }
    }

    private void PlayerInputs()
    {
        //Look left and right
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //adds forward velocity
            if (Input.GetKey(KeyCode.W))
            {               
                rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
                up = true;
            }
            //adds backward velocity
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.forward * speed, ForceMode.VelocityChange);
                down = true;
            }
            //adds velocity to the left
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-transform.right * speed, ForceMode.VelocityChange);
                left = true;
            }
            //adds velocity to the right
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * speed, ForceMode.VelocityChange);
                right = true;
            }
        }
        //stop velocity when button is unpressed
        else
        {
            //stops velocity emidiatly when key is unpressed
            if (up == true)
            {
                rb.velocity = Vector3.zero;
                up = false;
            }

            if (down == true)
            {
                rb.velocity = Vector3.zero;
                down = false;
            }

            if (left == true)
            {
                rb.velocity = Vector3.zero;
                left = false;
            }

            if (right == true)
            {
                rb.velocity = Vector3.zero;
                right = false;
            }
        }
        //Jump
        if (Input.GetKey(KeyCode.Space))
        {
            //Sends out a Raycast and puts it in hit
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, groundDetectionDistance, layerMask))
            {
                //Allows you  to jump when the raycast hits an object with the tag ground
                if (hit.collider.tag == "Ground")
                {
                    rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
                }
            }
        }
        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            maxSpeed = 15f;
        }
        else
        {
            maxSpeed = 7.5f;
        }
    }
}