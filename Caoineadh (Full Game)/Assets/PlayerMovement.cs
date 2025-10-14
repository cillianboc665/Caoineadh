using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
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

    }
}