using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Animation
    Animator animation;
    const string idleAnimation = "Idle";
    const string runAnimation = "Run";
    const string jumpAnimation = "Jump";
    const string attackAnimation = "Attack";
    const string kickAttackAnimation = "AttackKick";
    private string currentState = "Idle";
    bool animationCheck = false;

    [Header("Player Speeds")]
    public float moveSpeed = 10; // Used to allow changing of the players speed
    public float jumpForce = 10; // Used to allow changing of the players jumpheight and speed
    private const float AirTurnSpeedMultiplier = 20;
    public float maxAirTurnSpeed = 5;
    public float maxAirSpeed = 10;
    public float wallSlidingSpeed = 2f;
    [SerializeField] private Vector2 wallJumpingPower = new(8f, 16f);
    private bool wallJumped = false;
    
    [Header("Ground Layer & Wall Layer + Ckeck")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer; // Used to store the wall layer 
    // Variables for making the wall jump work. The method comes from this video: https://www.youtube.com/watch?v=O6VX6Ro7EtA&t=237s
    

    

     // Used to ensure that what the player is hitting is a wall, it is an empty gameobject put on the player, that is used in the WallSlide method

    
    private bool isFacingRight = true; // Used for flipping the player left or right
    private bool isGrounded; // Used for checking if the player is touching the ground

    private bool isWallJumping;
    private bool isWallSliding;

 

    [Header("Dash")]
    [SerializeField] private float dashingVelocityX = 200; // Velocity of the dash, Serialized for easy changes
    // [SerializeField] private float dashingVelocityY = 10f;
    [SerializeField] private float dashingTime = 0.1f; // How long is the dash gonna last? Serialized for easy changes
    private Vector2 dashingDir; // Storing the direction of the dash
    private bool isDashing; // Checking if the character is dashing
    private bool canDash = true; // Checking if the character is in a state where they CAN dash 
    private bool jumpCheck; // This variable is used to check if the jump button is pressed
    private float moveInput; // Used for storing the movement input, so that it can be taken from update to fixedupdate
    private TrailRenderer dashRender; // Storing the trail renderer for the dash

    private Rigidbody2D rb2D;
    private float wallJumpingCounter;
    private float wallJumpingDirection;
    private readonly float wallJumpingDuration = 0.4f;
    private readonly float wallJumpingTime = 0.01f;


    // Start is called before the first frame update
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        dashRender = GetComponent<TrailRenderer>();
        animation = GetComponent<Animator>();
    }


    // Update is called once per frame
    private void Update()
    {


        //---------------------------------
        //          Animation
        //---------------------------------
        AnimationParameters();
        RunningAnimationControl();
        Debug.Log(animationCheck);
        //---------------------------------

        // Since update is the fastest way to check if something is happening, the input checks are placed here to make the game feel more repsonsive
        moveInput = Input.GetAxis("Horizontal");



        CheckIfButtonPressed(); // Checks if a button has been pressed
        CheckIfGrounded(); // Checks if the player is on the ground

        WallSlide(); // Checks whether the player should be sliding down a wall
        WallJump(); // Allows the player to jump off a wall


        if (!isWallJumping)
            Flip();

        // ----------------DASH------------
        float dashDirX = Input.GetAxis("Horizontal"); // Horizontal direction!!
     // float dashDirY = Input.GetAxis("Vertical"); // Vertical direction!!
     // Since update is the fastest way to check if something is happening, the input checks are placed here to make the game feel more repsonsive
        moveInput = Input.GetAxis("Horizontal");


        var dashInput = Input.GetButtonDown("Dash"); // Get the dash input from the input manager

        if (dashInput && canDash) // If the input for the dash is pressed and you can dash...
        {
            isDashing = true; // .. then you will be set to "is dashing" aaand...
            canDash = false; // ... can dash is set to false!



            dashRender.emitting = true; // Dash rederer will start emitting!
            dashingDir = new Vector2(dashDirX, 0); // And the direction is based on the input manager so directions from there



            if (dashingDir == Vector2.zero) // if were pressing the dash button but no direction 
            {
                dashingDir = new Vector2(transform.localScale.x, 0); // then it will go by the local scale of the x axis and not use the y axis, localscale depends on which way your char is facing!
            }
            StartCoroutine(StopDashing());
        }


        if (isDashing && dashDirX < 1)
        {
            rb2D.AddForce(dashingDir.normalized * dashingVelocityX);
            Debug.Log(dashingDir.normalized * dashingVelocityX);
            return;
        }


        if (isGrounded)
        {
            canDash = true;
            wallJumped = false;
        }

}

    private void FixedUpdate()
    {
        // Move and jump are placed here to ensure that they are triggered with the physics engine

        Move();



        // Used to check if the player can jump and they have pressed the jump button
        if (jumpCheck && isGrounded)
        {
            // This if statement determines if the player is allowed to jump, jump check is used to check if the player has pressed the jump button and
            // isGrounded is used to check if the player is touching the ground
            ChangeAnimationState(jumpAnimation);
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

    private void Move()
    {
        if (isGrounded){
            // Used to make the player move
            rb2D.velocity = new Vector2(moveInput * moveSpeed, rb2D.velocity.y);
        }
        if(!isGrounded&&!IsWalled()&&!wallJumped)
        {
            rb2D.AddForce(new Vector2(((Mathf.Clamp(moveInput*AirTurnSpeedMultiplier,-maxAirTurnSpeed,maxAirTurnSpeed))),0));
            rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -maxAirSpeed, maxAirSpeed), rb2D.velocity.y);
            //new Vector2(Mathf.Clamp((Mathf.Clamp(moveInput,-1f,1f))*4,-10f,10f),rb2D.velocity.y);
        }
    }

    private void CheckIfButtonPressed()
    {
        // This method would be used to check for specific button presses. I imagine this could be used to check for other buttons presses too, if that should be necessary
        if (isGrounded)
            if (Input.GetKeyDown(KeyCode.Space))
                jumpCheck = true;
    }

    private void CheckIfGrounded()
    {
        // This method is used to check if the player is on the ground. It uses a collider object on the player prefab, that checks if it collides with an object set to the ground layer
        // The method returns true if this is the case
        // If the player is on the ground, they are allowed to jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheck.GetComponent<CircleCollider2D>().radius,
            groundLayer);
    }


    // These methods deal with wall Sliding and Wall jumping
    private bool IsWalled()
    {
        return
            Physics2D.OverlapCircle(wallCheck.position, wallCheck.GetComponent<CircleCollider2D>().radius,
                wallLayer); // Creates a small circle around the wallcheck object on the player that checks if it is hitting a gameobject on the layer "WAll"
    }


    private void WallSlide()
    {
        if (IsWalled() && !isGrounded && moveInput != 0f)
        {
            // Here is checkked if the isWalled methods returns true and the player is not grounded and their current move input is not 0, meaning they are pressing a direction
            if(Input.GetKeyDown(KeyCode.Space)){
                ChangeAnimationState(jumpAnimation);
            }
            isWallSliding = true; // Sets the wallSliding to true
            rb2D.velocity = new Vector2(rb2D.velocity.x,
                Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed,
                    float.MaxValue)); // Changes the players velocity, so they start "falling" slowly, but only so long as they are touching a wall
        }
        else
        {
            isWallSliding = false; // Sets the wallsliding to false if the player is not touching a wall
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            // This if else statement is there to give the player a short window of time after they have stopped touching the wall to still walljump, making the jump feel a little more forgiving

            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x; // Sets the wallJump direction to the opposite of the players current direction.
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping)); // Cancels the stop of the wallJump if the player is sliding on a wall, since we want the player to be cabable of wallJumping
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime; // this counts down the counter, so that the player has a short window of time to wallJump even after they are no longer touching the wall
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            // Checks if the player presses the jump button and if the wallJumpingCounter is greater than zero, since that means the player is allowed to wallJump
            wallJumped = true;
            isWallJumping = true;
            rb2D.velocity =
                new Vector2(wallJumpingDirection * wallJumpingPower.x,
                    wallJumpingPower.y); // Used to handle the players jump direction and velocity
            wallJumpingCounter =
                0f; // Since the player cannot wallJump again after they have already jumped, this sets that counter to zero

            if (transform.localScale.x != wallJumpingDirection)
            {
                // This part of the code ensures that the player is facing the correct direction following a walljump. Might be redundant due to how the players movement is setup
                isFacingRight = !isFacingRight;
                var localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping),
                wallJumpingDuration); // Stops the player from wallJumping if they are no longer touching the wall after a short delay
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false; // Stops the player from wall jumping after they have already done so
    }

    // TODO: Make this code work
    private void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            // The if statement here checks whether the player is inputting left or right and whether the isFacingRight is true or false
            // The video where i got this from: https://www.youtube.com/watch?v=K1xZ-rycYY8
            isFacingRight = !isFacingRight;
            var localScale = transform.localScale; // The players localScale, can be viewed under the players transform
            localScale.x *= -1f; // Flips the current localScale value to the opposite
            transform.localScale = localScale; // Flips the players localScale
        }
    }

    private void RunningAnimationControl(){
        if(moveInput==0&&isGrounded){
            ChangeAnimationState(idleAnimation);
        }else if (moveInput != 0&&isGrounded){
            ChangeAnimationState(runAnimation);
        }
    }

    private void AnimationParameters()
    {
       
        // Holder øje med Chilis y.velocity
        animation.SetFloat("yVelocity",rb2D.velocity.y);

        // Animation Parameter som ser om Chili er grounded
        if(isGrounded){
            animation.SetBool("isGrounded", true);
        }else{
            animation.SetBool("isGrounded", false);
        }

        // Animation Parameter som tjekker for spiller-input
        if(moveInput==0){
            animation.SetBool("movementInput", false);
        }else{
             animation.SetBool("movementInput", true);
        }

        // Animation Parameter som tjekker om Chili rør en væg
        if(IsWalled()){
            animation.SetBool("isWalled",true);
        } else {
            animation.SetBool("isWalled",false);
        }
        
        // Animation Parameter som tjekker om man angriber og hvor mange gange man trykker
        if(Input.GetKeyDown(KeyCode.P)&&!animationCheck)
        {
            ChangeAnimationState(attackAnimation);
        }
        
         try
        {
            if(animation.GetCurrentAnimatorClipInfo(0)[0].clip.name==attackAnimation)
            {
                animationCheck = true;
            }   
            else if (animation.GetCurrentAnimatorClipInfo(0)[0].clip.name!=attackAnimation&&animation.GetCurrentAnimatorClipInfo(0)[0].clip.name!=kickAttackAnimation)
            {
                animationCheck = false;
            }
            
            if(animationCheck&&Input.GetKeyDown(KeyCode.P))
            {
            animation.SetBool("QuedAttack",true);
            }

            if(animation.GetCurrentAnimatorClipInfo(0)[0].clip.name==kickAttackAnimation)
            {
                animation.SetBool("QuedAttack",false);
                animationCheck = false;
            }
                

        }
        catch (System.IndexOutOfRangeException e)
        {
            System.Console.WriteLine("An Error has occurred : {0}",e.Message);
        }
    }
        

        

    private void ChangeAnimationState(string newState)
    {
        if(currentState == newState) return;

        animation.Play(newState);

        currentState = newState;
    }



}