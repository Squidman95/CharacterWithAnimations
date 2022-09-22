using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;       // Could keep track of up/down direction with moveDirection, but this is a cleaner way to keep track of gravity/jumping

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance; // groundCheckDistance = radius of our sphere. Size of the ground check
    [SerializeField] private LayerMask groundMask;      // groundMask is a layer to check for if we're grounded. Ground will be on a separate layer, to ensure that we can only jump when we're on the ground.
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;


    // REFERENCES
    private CharacterController controller;
    private Animator animator;

    private void Start()
    {
        Debug.Log("Game Started");
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); // transform.position = position of our playerfeet | Function will return true when we're standing on the ground, and false when we're not.

        if(isGrounded && velocity.y < 0)    // If we're grounded, stop applying gravity.
        {
            velocity.y = -2f;               // If we set to 0, it might not ground us fully, so we set to a small negative value.
        }

        // THIS IS TO MOVE BOTH HORIZONTAL AND VERTICAL! HAVE TO SET MOVESPEED TO 1 IN EDITOR TO WORK
        float moveZ = Input.GetAxis("Vertical");    // When we click W it's set to 1 and S will set to -1
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX * moveSpeed, moveDirection.y, moveZ * moveSpeed);
        moveDirection = transform.TransformDirection(moveDirection);

        // THIS IS TO MOVE ONLY VERTICAL! MOVESPEED CAN BE 0 IN EDITOR AND WORK
        //float moveZ = Input.GetAxis("Vertical");    // When we click W it's set to 1 and S will set to -1
        //moveDirection = new Vector3(0, 0, moveZ);
        //moveDirection = transform.TransformDirection(moveDirection);    // Makes it so "forward" is whatever the player forward is


        if (isGrounded)  // Ensure that we can only move when we're grounded
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))   // If we're moving - moveDirection is not (0, 0, 0) and LeftShift is NOT down
            {   // Walk
                Debug.Log("Walking");
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) // If we're moving and LeftShift IS down
            {   // Run
                Debug.Log("Running");
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {   // Idle
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jumping");
                Jump();
            }
        }
        


        controller.Move(moveDirection * Time.deltaTime); // Time.deltaTime ensures no matter how many frames we have, we move the same speed.

        velocity.y += gravity * Time.deltaTime;     // Calculate gravity
        controller.Move(velocity * Time.deltaTime); // Apply gravity to our character.
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.15f, Time.deltaTime);    // 0.1rf, Time.deltaTime | this makes it more smooth
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.15f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1, 0.15f, Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); // Formular for gravity.
    }

    //private void Attack()
    //{
    //    animator.SetTrigger("Attack");
    //}

    private IEnumerator Attack()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1); // When we attack we set the attack layer weight for 1
        animator.SetTrigger("Attack");                                      // Play animation

        yield return new WaitForSeconds(0.9f);                              // Wait for 0.9 seconds
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0); // Set layer weight to 0 (disable the layer)
    }

}
