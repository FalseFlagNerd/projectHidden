using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------- FPSController: MonoBehaviour -------------------
   |  Purpose:  Performs the necessary calls to the FPSMovement script to perform  
   |            movement, forces, jumps, etc.     
   |
   |  Author: Jordan Pitner
   |  Date: August 7th, 2018
   *-------------------------------------------------------------------*/
[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour {

    #region Global Variables

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float sprintSpeed = 8f;

    [SerializeField]
    private float crouchSpeed = 2.5f;

    [SerializeField]
    private float jumpForce = 2.0f; 

    [SerializeField]
    private float lookSensitivity = 5f;

    [SerializeField]
    private float crouchHeight = .25f;

    private bool isGrounded;
    private bool isCrouched;
    private bool isSprinting;
    private Vector3 velocity;

    private FPSMovement movement;

    #endregion

    // Use this for initialization
    void Start ()
    {
        movement = GetComponent<FPSMovement>();	
        
	}

    /*-------------------------- OnCollisionStay() -------------------
  |  Function OnCollisionStay
  |
  |  Purpose:  A check to see if the player is colliding with the ground
  |
  |  Parameters: None
  |	      
  |  Returns:  Nothing
  *-------------------------------------------------------------------*/
    void OnCollisionStay()
    {
        isGrounded = true;
    }

    /*-------------------------- Update() -------------------
  |  Function Update
  |
  |  Purpose:  Standard update function to continuously make movement checks/calls
  |
  |  Parameters: None
  |	      
  |  Returns:  Nothing
  *-------------------------------------------------------------------*/
    void Update () 
    {      
        // Check the state the character is in for movement properties
        CheckCharacterMovementState();
        
        // Calculate and set the velocity vector for movement
        CalculateMovement();

        // Calculate and set the rotation variables for movement
        CalculateRotation();  
    }

    /*-------------------------- CalculateMovement() -------------------
  |  Function CalculateMovement
  |
  |  Purpose:  Function to calculate the velocity of the player and performs movement
  |
  |  Parameters: None
  |	      
  |  Returns:  Nothing
  *-------------------------------------------------------------------*/
    private void CalculateMovement()
    {
        // Calculate movement as 3D vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * yMov;

        // Checks to see what state the character is in for movement speed
        if (isSprinting)
        {
            velocity = (movHorizontal + movVertical).normalized * sprintSpeed;
        }
        else if (isCrouched)
        {
            velocity = (movHorizontal + movVertical).normalized * crouchSpeed;
        }
        else
        {
            velocity = (movHorizontal + movVertical).normalized * speed;
        }

        // Apply movement
        movement.Move(velocity);
    }

    /*-------------------------- CalculateRotation() -------------------
  |  Function CalculateRotation
  |
  |  Purpose:  Function to calculate the rotation of the player and camera
  |
  |  Parameters: None
  |	      
  |  Returns:  Nothing
  *-------------------------------------------------------------------*/
    private void CalculateRotation()
    {
        // Calculate rotation as a new vector
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        movement.Rotate(rotation);

        // Calculate camera rotation as new future euler angles
        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * lookSensitivity;
        movement.RotateCamera(cameraRotationX);
    }

    /*-------------------------- CheckCharacterMovementState() -------------------
  |  Function CheckCharacterMovementState
  |
  |  Purpose:  Function to check whether the player is jumping, crouching, sprinting, etc
  |
  |  Parameters: None
  |	      
  |  Returns:  Nothing
  *-------------------------------------------------------------------*/
    private void CheckCharacterMovementState()
    {
        // Check to see if any character state keys are being pressed
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            movement.Crouch(new Vector3(0, -crouchHeight, 0));

            isCrouched = true; // Crouched
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            movement.Crouch(new Vector3(0, crouchHeight, 0));

            isCrouched = false; // Not crouched
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true; // Sprinting
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false; // Not sprinting
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            movement.Jump(jumpForce);

            isGrounded = false; // Jumping
        }
    }
}
