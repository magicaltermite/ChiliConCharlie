using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  
    public float moveSpeed = 10;    // Used to allow changing of the players speed
    public float jumpForce = 10;    // Used to allow changing of the players jumpheight and speed

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb2D;

    private bool jumpCheck;         // This variable is used to check if the jump button is pressed
    private float moveInput;        // Used for storing the movement input, so that it can be taken from update to fixedupdate
    private bool isGrounded;        // Used for checking if the player is touching the ground

    private TrailRenderer dashRender; // Storing the trail renderer for the dash

    [Header("Dash")]
    [SerializeField] private float dashingVelocityX = 14f; // Velocity of the dash, Serialized for easy changes
    [SerializeField] private float dashingVelocityY = 10f;
    [SerializeField] private float dashingTime = 0.5f; // How long is the dash gonna last? Serialized for easy changes
    private Vector2 dashingDir; // Storing the direction of the dash
    private bool isDashing; // Checking if the character is dashing
    private bool canDash = true; // Checking if the character is in a state where they CAN dash 


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        dashRender = GetComponent<TrailRenderer>(); 
    }



    // Update is called once per frame
    void Update() {
        float dashDirX = Input.GetAxis("Horizontal"); // Horizontal direction!!
        float dashDirY = Input.GetAxis("Vertical"); // Vertical direction!!
        // Since update is the fastest way to check if something is happening, the input checks are placed here to make the game feel more repsonsive
        moveInput = Input.GetAxis("Horizontal");
        
        CheckIfButtonPressed(); // Checks if a button has been pressed
        CheckIfGrounded();      // Checks if the player is on the ground

        var dashInput = Input.GetButtonDown("Dash"); // Get the dash input from the input manager

        if (dashInput && canDash ) // If the input for the dash is pressed and you can dash...
        {
            isDashing = true; // .. then you will be set to "is dashing" aaand...
            canDash = false; // ... can dash is set to false!
            


            dashRender.emitting = true; // Dash rederer will start emitting!
            dashingDir = new Vector2(dashDirX, dashDirY); // And the direction is based on the input manager so directions from there

            

            if(dashingDir == Vector2.zero) // if were pressing the dash button but no direction 
            {
                dashingDir = new Vector2(transform.localScale.x, 0); // then it will go by the local scale of the x axis and not use the y axis, localscale depends on which way your char is facing!
            }
            StartCoroutine(StopDashing());
        }


        if (isDashing && dashDirX<1)
        {
            rb2D.AddForce(dashingDir.normalized * dashingVelocityX);
            Debug.Log(dashingDir.normalized * dashingVelocityX);
            return; 
        }
        if (isDashing && dashDirY< 1)
        {
            rb2D.AddForce(dashingDir.normalized * dashingVelocityY);
            Debug.Log(dashingDir.normalized * dashingVelocityY);
            return;
        }

        if (isGrounded)
        {
            canDash = true; 
        }
    }

    void FixedUpdate() {
        // Move and jump are placed here to ensure that they are triggered with the physics engine
        Move();

        // Used to check if the player can jump and they have pressed the jump button
        if (jumpCheck == true && isGrounded == true) { // This if statement determines if the player is allowed to jump, jump check is used to check if the player has pressed the jump button and
                                                       // isGrounded is used to check if the player is touching the ground

            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCheck = false;
        }
        
    }

    private IEnumerator StopDashing() // Coroutine for when the dash is stopping
    {
        yield return new WaitForSeconds(dashingTime); // Wait for the amount of time a dash takes
        Move();
        dashRender.emitting = false; // Set the render emitting to false
        isDashing = false; // and change the is dashing state from true to false
    }

    private void Move() {
        // Used to make the player move
        rb2D.velocity = new Vector2(moveInput * moveSpeed, rb2D.velocity.y);
    }

    private void CheckIfButtonPressed() {
        // This method would be used to check for specific button presses. I imagine this could be used to check for other buttons presses too, if that should be necessary

        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpCheck = true;
        }
    }

    private void CheckIfGrounded() {
        // This method is used to check if the player is on the ground. It uses a collider object on the player prefab, that checks if it collides with an object set to the ground layer
        // The method returns true if this is the case
        // If the player is on the ground, they are allowed to jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheck.GetComponent<CircleCollider2D>().radius, groundLayer);
        
    }
}
