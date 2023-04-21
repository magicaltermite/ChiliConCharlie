using UnityEngine;

public class TestScript : MonoBehaviour
{
    //Basic Player Movement - Move Left & Right, Jump, Ground Check + Flip()
    public float speed;
    public float jumpForce;
    public float checkRadius;
    
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float _input;
    private bool _isGrounded;
    private bool _facingRight = true;
    
    private Rigidbody2D _rb;

    private void Move()
    {
        GroundCheck();
        _rb.velocity = new Vector2(_input * speed, _rb.velocity.y);
        if (_input > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_input < 0 && _facingRight)
        {
            Flip();
        }

        // Jump

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity = Vector2.up * jumpForce;
        }
    }
    private void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }
    private void Flip() {
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale = new Vector3(-localScale.x,localScale.y,localScale.z);
        transform1.localScale = localScale;
        _facingRight = !_facingRight;
    }
    
    //-----------------------------------------------------------------------------------------------------------------
    
    // Wall Sliding Variables and Methods

    public float wallSlideSpeed;

    public Transform wallCheck;
    public LayerMask wallLayer;
    
    private bool _isWalled;
    private bool _isWallSliding;

    
    void WallSlide()
    {
        WallCheck();
        WallSlidingCheck();

        if (_isWallSliding)
        {
            var velocity = _rb.velocity;
            _rb.velocity = new Vector2(velocity.x,Mathf.Clamp(velocity.y,-wallSlideSpeed,float.MaxValue));
        }
    }
    void WallSlidingCheck()
    {
        if (_isWalled && !_isGrounded && _input != 0)
            _isWallSliding = true;
        else
            _isWallSliding = false;
    }
    void WallCheck()
    {
        _isWalled = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);
    }

    
    // Wall Jumping Variables and Methods

    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    
    private bool _isWallJumpingReady;

    void WallJump()
    {
        WallJumpReadyCheck();
        if (_isWallJumpingReady)
        {
            _rb.velocity = new Vector2(xWallForce * -_input, yWallForce);
        }
    }
    void WallJumpReadyCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isWallSliding)
        {
            _isWallJumpingReady = true;
            Invoke(nameof(WallJumpReadyReset),wallJumpTime);
        }
    }
    void WallJumpReadyReset()
    {
        _isWallJumpingReady = false;
    }
    //-----------------------------------------------------------------------------------------------------------------
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _input = Input.GetAxis("Horizontal");
        
        Move();
        WallSlide();
        WallJump();

    }
    private void FixedUpdate()
    {

    }
}