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
    private bool isFacingRight = true;  // Used for flipping the player left or right

    // Variables for making the wall jump work. The method comes from this video: https://www.youtube.com/watch?v=O6VX6Ro7EtA&t=237s
    public float wallSlidingSpeed = 2f;
    private bool isWallSliding;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(200f, 16f);

    [SerializeField] private Transform wallCheck; // Used to ensure that what the player is hitting is a wall, it is an empty gameobject put on the player, that is used in the WallSlide method
    [SerializeField] private LayerMask wallLayer; // Used to store the wall layer 




    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update() {

        // Since update is the fastest way to check if something is happening, the input checks are placed here to make the game feel more repsonsive
        moveInput = Input.GetAxis("Horizontal");
        
        CheckIfButtonPressed(); // Checks if a button has been pressed
        CheckIfGrounded();      // Checks if the player is on the ground

        WallSlide();            // Checks whether the player should be sliding down a wall
        WallJump();             // Allows the player to jump off a wall


        if(!isWallJumping) 
            Flip();

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

    private void Move() {
        // Used to make the player move
        rb2D.velocity = new Vector2(moveInput * moveSpeed, rb2D.velocity.y);
    }

    private void CheckIfButtonPressed() {
        // This method would be used to check for specific button presses. I imagine this could be used to check for other buttons presses too, if that should be necessary
        if(isGrounded){
            if (Input.GetKeyDown(KeyCode.Space)) {
                jumpCheck = true;
            }
        }
    }

    private void CheckIfGrounded() {
        // This method is used to check if the player is on the ground. It uses a collider object on the player prefab, that checks if it collides with an object set to the ground layer
        // The method returns true if this is the case
        // If the player is on the ground, they are allowed to jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheck.GetComponent<CircleCollider2D>().radius, groundLayer);
        
    }


    // These methods deal with wall Sliding and Wall jumping
    private bool IsWalled() {
        return Physics2D.OverlapCircle(wallCheck.position, 0.3f, wallLayer); // Creates a small circle around the wallcheck object on the player that checks if it is hitting a gameobject on the layer "WAll"
    }


    private void WallSlide() {

        if(IsWalled() && !isGrounded && !isWallJumping) { // Here is checkked if the isWalled methods returns true and the player is not grounded and their current move input is not 0, meaning they are pressing a direction
            
            isWallSliding = true; // Sets the wallSliding to true
            rb2D.velocity = new Vector2(0, Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed, float.MaxValue)); // Changes the players velocity, so they start "falling" slowly, but only so long as they are touching a wall
            Debug.Log("Sliding");
        }
        else {
            isWallSliding = false; // Sets the wallsliding to false if the player is not touching a wall
        }

    }

    private void WallJump() {

        // This if else statement is there to give the player a short window of time after they have stopped touching the wall to still walljump, making the jump feel a little more forgiving
        
        if(isWallSliding) { 

            isWallJumping = false; 
            wallJumpingDirection = -transform.localScale.x; // Sets the wallJump direction to the opposite of the players current direction.
            wallJumpingCounter = wallJumpingTime;           

            CancelInvoke(nameof(StopWallJumping)); // Cancels the stop of the wallJump if the player is sliding on a wall, since we want the player to be cabable of wallJumping
        }
        else {

            wallJumpingCounter -= Time.deltaTime; // this counts down the counter, so that the player has a short window of time to wallJump even after they are no longer touching the wall
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f) { // Checks if the player presses the jump button and if the wallJumpingCounter is greater than zero, since that means the player is allowed to wallJump

            isWallJumping = true;
            Debug.Log("Jump");
            rb2D.AddForce(new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y),ForceMode2D.Impulse);
            wallJumpingCounter = 0f;                                                                    // Since the player cannot wallJump again after they have already jumped, this sets that counter to zero

            if (transform.localScale.x != wallJumpingDirection) { // This part of the code ensures that the player is facing the correct direction following a walljump. Might be redundant due to how the players movement is setup
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration); // Stops the player from wallJumping if they are no longer touching the wall after a short delay

        }
    }

    private void StopWallJumping() {
        isWallJumping = false; // Stops the player from wall jumping after they have already done so
    }

    // TODO: Make this code work
    private void Flip() {

        if(isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f) { // The if statement here checks whether the player is inputting left or right and whether the isFacingRight is true or false
            // The video where i got this from: https://www.youtube.com/watch?v=K1xZ-rycYY8
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale; // The players localScale, can be viewed under the players transform
            localScale.x *= -1f; // Flips the current localScale value to the opposite
            transform.localScale = localScale; // Flips the players localScale
        }
    }
}
