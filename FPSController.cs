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
    private float jumpForce = 2.0f; 

    [SerializeField]
    private float lookSensitivity = 5f;

    private bool isGrounded; 

    private FPSMovement movement;

    #endregion

    // Use this for initialization
    void Start ()
    {
        movement = GetComponent<FPSMovement>();	
        
	}
	
    void OnCollisionStay()
    {
        isGrounded = true;
    }

	// Update is called once per frame
	void Update () 
    {
        Vector3 velocity; 

        // Calculate movement as 3D vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * yMov;

        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            velocity = (movHorizontal + movVertical).normalized * sprintSpeed;
        }
        else
        {
            velocity = (movHorizontal + movVertical).normalized * speed;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            movement.Jump(jumpForce);
            isGrounded = false;
        }

        // Apply movement
        movement.Move(velocity);

        // Calculate rotation as a new vector
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        movement.Rotate(rotation);

        // Calculate camera rotation as new future euler angles
        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * lookSensitivity;
        movement.RotateCamera(cameraRotationX);
    }
}
