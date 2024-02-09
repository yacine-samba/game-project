using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController characterController;

    //Walking speed
    public float walkingSpeed = 2f;

    //Running speed
    public float runningSpeed = 4f;

    //Jump speed
    public float jumpSpeed = 6f;

    //Gravity
    float gravity = 20f;

    //Moving
    Vector3 moveDirection;

    //Walk or run?
    private bool isRunning = true;

    private Animator animator;

    void Start()
    {
        // Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        float verticalSpeed = Input.GetAxis("Vertical");

        float horizontalSpeed = Input.GetAxis("Horizontal");

        float jumpMovement = moveDirection.y;


        // Movement rotation player on walking
        var targetAngle = Mathf.Atan2(horizontalSpeed, verticalSpeed) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        var thisAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref moveDirection.x, 0.1f);
        transform.rotation = Quaternion.Euler(0, thisAngle, 0);
        
        
        // Animation input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Running
            isRunning = true;
            animator.SetBool("run", true);
        }
        else
        {
            //Walking
            isRunning = false;
            animator.SetBool("run", false);
        }

        if (isRunning)
        {
            //Multiply speed by running speed
            horizontalSpeed = horizontalSpeed * runningSpeed;
            verticalSpeed = verticalSpeed * runningSpeed;
        }
        else
        {
            //Multiply speed by walking speed
            horizontalSpeed = horizontalSpeed * walkingSpeed;
            verticalSpeed = verticalSpeed * walkingSpeed;
        }

        //Motion calculation
        //forward = front/rear axis
        //right = left/right axis
        moveDirection = forward * verticalSpeed + right * horizontalSpeed;


        // Press Jump ?
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {

            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = jumpMovement;
        }

        //If the player does not touch the ground
        if (!characterController.isGrounded)
        {
            //Apply gravity * deltaTime
            //Time.deltaTime = Time since last frame
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

}
