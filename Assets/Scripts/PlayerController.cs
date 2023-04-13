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


    public float cooldownTime = 0.5f; // The cooldown time for the bullet
    private bool canShoot = true;
    [SerializeField] private GameObject bulletprefab;




    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        // Since update is the fastest way to check if something is happening, the input checks are placed here to make the game feel more repsonsive
        moveInput = Input.GetAxis("Horizontal");

        CheckIfButtonPressed(); // Checks if a button has been pressed
        CheckIfGrounded();      // Checks if the player is on the ground

        if (canShoot && Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
            StartCoroutine(Cooldown());
        }
    }

    void FixedUpdate()
    {
        // Move and jump are placed here to ensure that they are triggered with the physics engine
        Move();

        // Used to check if the player can jump and they have pressed the jump button
        if (jumpCheck == true && isGrounded == true)
        { // This if statement determines if the player is allowed to jump, jump check is used to check if the player has pressed the jump button and
          // isGrounded is used to check if the player is touching the ground

            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCheck = false;
        }

    }

    private void Move()
    {
        // Used to make the player move
        rb2D.velocity = new Vector2(moveInput * moveSpeed, rb2D.velocity.y);
    }

    private void CheckIfButtonPressed()
    {
        // This method would be used to check for specific button presses. I imagine this could be used to check for other buttons presses too, if that should be necessary

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpCheck = true;
        }
    }

    private void CheckIfGrounded()
    {
        // This method is used to check if the player is on the ground. It uses a collider object on the player prefab, that checks if it collides with an object set to the ground layer
        // The method returns true if this is the case
        // If the player is on the ground, they are allowed to jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheck.GetComponent<CircleCollider2D>().radius, groundLayer);

    }

    // Shoot method
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletprefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(10f, 0f);

       
    }

    //Coroutine, cooldown for the bullet
    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }

}