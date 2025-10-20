using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* [Header("Movement")]
     public float moveSpeed;

     public float groundDrag;

     public float jumpForce;
     public float jumpCooldown;
     public float airMarkiplier;
     bool readyToJump;

     [Header("Keybinds")]
     public KeyCode jumpKey = KeyCode.Space;

     [Header("GroundCheck")]
     public float playerHeight;
     public LayerMask whatIsGround;
     bool grounded;

     public Transform orientation;

     float horizInput;
     float vertiInput;

     Vector3 moveDir;

     Rigidbody rb;

     private int jumpCount;
     public int maxJumps = 1;

     public AudioSource audioSource; // attach the player's audio source
     public AudioClip[] dirtSteps;
     public AudioClip[] stoneSteps;
     public AudioClip[] grassSteps;
     public AudioClip[] leaveSteps;
     public float stepInterval = 0.5f;
     private float stepTimer;
     private string groundTag = "Default";
     public float crouchHeight = 1f;
     public float crouchSpeed = 3f;
     public float walkSpeed = 6f;
     public float runSpeed = 12f;
     public float jumpPower = 0f;
     public float gravity = 10f;
     public float lookSpeed = 2f;
     public float lookXLimit = 45f;
     public float defaultHeight = 2f;

     private bool canMove = true;

     private CharacterController characterController;


     private void Start()
     {
         rb = GetComponent<Rigidbody>();
         rb.freezeRotation = true;

         readyToJump = true;
     }

     private void Update()
     {
         RaycastHit hit;

         if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, whatIsGround))
         {
             grounded = true;
             rb.drag = groundDrag;
             jumpCount = 0;

             groundTag = hit.collider.tag;

             stepTimer += Time.deltaTime;
             if (stepTimer >= stepInterval && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
             {
                 Footsteps();
                 stepTimer = 0f;
             }
         }
         else
         {
             grounded = false;
             rb.drag = 0;
             stepTimer = 0f;
         }

         MyInput();

         if (Input.GetKey(KeyCode.LeftControl) && canMove)
         {
             characterController.height = crouchHeight;
             walkSpeed = crouchSpeed;
             runSpeed = crouchSpeed;

         }
         else
         {
             characterController.height = defaultHeight;
             walkSpeed = 6f;
             runSpeed = 12f;
         }

     }

     private void FixedUpdate()
     {
         MovePlayer();
     }
     private void MyInput()
     {
         horizInput = Input.GetAxisRaw("Horizontal");
         vertiInput = Input.GetAxisRaw("Vertical");


         if (Input.GetKeyDown(jumpKey) && jumpCount < maxJumps && readyToJump)
         {
             readyToJump = false;

             Jump();
             jumpCount++;

             Invoke(nameof(ResetJump), jumpCooldown);
         }
     }

     private void MovePlayer()
     {
         moveDir = orientation.forward * vertiInput + orientation.right * horizInput;

         if (grounded)
             rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);

         else if (!grounded)
             rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMarkiplier, ForceMode.Force);
     }

     private void SpeedControl()
     {
         Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

         if (flatvel.magnitude > moveSpeed)
         {
             Vector3 limitedVel = flatvel.normalized * moveSpeed;
             rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
         }
     }

     private void Jump()
     {
         rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

         rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
     }

     private void ResetJump()
     {
         readyToJump = true;
     } */



    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float groundDrag;
    public float stepInterval = 0.5f;
    private float stepTimer;


    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 0f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    private string groundTag = "Default";

    public AudioSource audioSource; // attach the player's audio source
    public AudioClip[] dirtSteps;
    public AudioClip[] stoneSteps;
    public AudioClip[] grassSteps;
    public AudioClip[] leaveSteps;
    public AudioClip[] woodSteps;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    public bool crouched;

    void Start()
    {

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, whatIsGround))
        {
            grounded = true;
            groundTag = hit.collider.tag;

            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                Footsteps();
                stepTimer = 0f;
            }
        }
        else
        {
            grounded = false;
            stepTimer = 0f;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
            crouched = true;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
            crouched = false;
        }

        if ((Input.GetKey(KeyCode.LeftShift)))
        {
            stepInterval = 0.27f;
        }
        else if (crouched)
        {
            stepInterval = 0.55f;
        }
        else
        {
            stepInterval = 0.46f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }



    private void Footsteps()
    {
        if (groundTag == "stone")
            audioSource.PlayOneShot(stoneSteps[Random.Range(0, stoneSteps.Length)]);

        else if (groundTag == "gras")
            audioSource.PlayOneShot(grassSteps[Random.Range(0, grassSteps.Length)]);

        else if (groundTag == "leaves")
            audioSource.PlayOneShot(leaveSteps[Random.Range(0, leaveSteps.Length)]);

        else if (groundTag == "dirt")
            audioSource.PlayOneShot(dirtSteps[Random.Range(0, dirtSteps.Length)]);

        else if (groundTag == "wood")
            audioSource.PlayOneShot(woodSteps[Random.Range(0, woodSteps.Length)]);

    }
}