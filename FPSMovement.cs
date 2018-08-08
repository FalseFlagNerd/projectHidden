using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------- FPSMovement: MonoBehaviour -------------------
   |  Purpose:  Performs and sets the necessary variables, forces, etc for 
   |            the player to be able to move and interact
   |
   |  Author: Jordan Pitner
   |  Date: August 7th, 2018
   *-------------------------------------------------------------------*/
[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour {

    // Global Variables
    #region Global Variables

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 crouch = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private float jumpForce = 0f; 

    private Rigidbody rigidBody;
    public Camera cam;

    #endregion

    /*-------------------------- Start() -------------------
   |  Function Start
   |
   |  Purpose:  Function to be called when the script first starts, sets globals
   |
   |  Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    /*-------------------------- Move(Vector3 velocity) -------------------
    |  Function Move
    |
    |  Purpose:  Set the velocity of the player movement
    |
    |  Parameters: Vector3 velocity
    |	      
    |  Returns:  Nothing
    *-------------------------------------------------------------------*/
    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }


    /*-------------------------- Rotate(Vector3 rotation) -------------------
    |  Function Rotate
    |
    |  Purpose:  Set the rotation of the player movement
    |
    |  Parameters: Vector3 rotation
    |	      
    |  Returns:  Nothing
    *-------------------------------------------------------------------*/
    public void Rotate(Vector3 rotation)
    {
        this.rotation = rotation; 
    }


    /*-------------------------- RotateCamera(Vector3 cameraRotationX) -------------------
   |  Function RotateCamera
   |
   |  Purpose:  Set the rotation of the camera movement
   |
   |  Parameters: Vector3 cameraRotationX
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    public void RotateCamera(float cameraRotationX)
    {
        this.cameraRotationX = cameraRotationX;
    }


    /*-------------------------- Jump(float jumpForce) -------------------
   |  Function Jump
   |
   |  Purpose:  Set the jumpForce float variable for player jumping
   |
   |  Parameters: float jumpForce
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    public void Jump(float jumpForce)
    {
        this.jumpForce = jumpForce;
        PerformJump();
    }

    /*-------------------------- Crouch(Vector3 crouch) -------------------
   |  Function Crouch
   |
   |  Purpose:  Set the crouch vector to apply the simulated crouch
   |
   |  Parameters: Vector3 crouch
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    public void Crouch(Vector3 crouch)
    {
        this.crouch = crouch;
        PerformCrouch();
    }

    /*-------------------------- FixedUpdate() -------------------
   |  Function FixedUpdate
   |
   |  Purpose:  Fixed calls to update based on frames, perform movement 
   |            and rotation functions consistently 
   |
   |  Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }


    /*-------------------------- PerformMovement() -------------------
   |  Function PerformMovement
   |
   |  Purpose:  Perform the actual player movement on the rigid body
   |
   |  Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    private void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
        }
    }


    /*-------------------------- PerformRotation() -------------------
   |  Function PerformRotation
   |
   |  Purpose:  Perform the actual rotation movement on the player rigid body
   |
   |  Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    private void PerformRotation()
    {
        rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(rotation));

        // Camera rotation calculation
        if (cam != null)
        {
            // Set the rotation and clamp it to limit rotation angles
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            // Apply the rotation transform to the player camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }


    /*-------------------------- PerformJump() -------------------
   |  Function PerformJump
   |
   |  Purpose:  Perform the actual jump and apply upward force to the player rigid body
   |
   |  Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    private void PerformJump()
    {
        Vector3 jump = new Vector3(0f, 2.0f, 0f);

        rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
    }

   /*-------------------------- PerformCrouch() -------------------
   |  Function PerformCrouch
   |
   |   Purpose:  Make the player/cam move down/up to simulate a crouch
   | 
   |   Parameters: None
   |	      
   |  Returns:  Nothing
   *-------------------------------------------------------------------*/
    private void PerformCrouch()
    {
        cam.transform.localPosition = crouch;
    }
}
