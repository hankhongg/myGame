using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector2 moveInput;

    public float wallJumpSpeed = 3f;

    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 8f;
    public float onAirWalkSpeed = 4f;
    public float onAirRunSpeed = 7f;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirection touchingDirection;


    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isCrouching = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _yVelocity = false;


    public float currentMoveSpeed
    {
        get
        {
            if (canMove)
            {
                if (touchingDirection.isGrounded)
                {
                    if (isMoving && !touchingDirection.isOnWall)
                    {
                        if (isRunning)
                        {
                            return runSpeed;
                        }
                        else return walkSpeed;
                    }
                    return 0;
                }
                else if (touchingDirection.isOnWall)
                {
                    return 0;
                }
                else
                {
                    if (isRunning)
                    {
                        return onAirRunSpeed;
                    }
                    return onAirWalkSpeed;
                }
            }
            return 0;
            
        }
    }

    public bool isCrouching
    {
        get
        {
            return _isCrouching;
        }
        private set
        {
            _isCrouching = value;
            animator.SetBool(AnimationStrings.crouch, value);
        }
    }

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool isMoving {  
        get {
            return _isMoving;
        }
        private set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }  
    }

    public bool isRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool yVelocity
    {
        get
        {
            return _yVelocity;
        }
        set
        {
            _yVelocity = value;
            animator.SetBool(AnimationStrings.yVelocity, value);
        }
    }

    public bool _isFacingRight = true;

    public bool isFacingRight {
        get
        {
            return _isFacingRight;
        } 
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    // whenever "this" is in the screen
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // happens once in a specific time
    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput.x * currentMoveSpeed /* * Time.fixedDeltaTime */, rb.velocity.y);
        if (touchingDirection.isGrounded || (!touchingDirection.isOnWall && rb.velocity.y <= 0))
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);


        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        isMoving = moveInput != Vector2.zero;

        setFacingDirection(moveInput);

    }

    private void flip()
    {
        transform.localScale *= new Vector2(-1, 1);
        isFacingRight = !isFacingRight;
    }
    private void setFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.isGrounded)
        {
            isRunning = true;
        } 
        else if (context.canceled) {
            isRunning = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.isGrounded && canMove)
        {
            animator.SetTrigger(AnimationStrings.jump);
            if (isRunning)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * (float)1.5);
            else
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void WallJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.isOnWall && !touchingDirection.isGrounded)
        {
            animator.SetTrigger(AnimationStrings.wallJump);
            float jumpDirection = touchingDirection.wallDirections.x > 0 ? -1 : 1;

            Vector2 wallJumpForce = new Vector2(jumpDirection * wallJumpSpeed, jumpForce);
            rb.AddForce(wallJumpForce, ForceMode2D.Impulse);


            if ((jumpDirection > 0 && !isFacingRight) || (jumpDirection < 0 && isFacingRight))
                isFacingRight = !isFacingRight;
        }

    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if  (context.started && touchingDirection.isGrounded && !isCrouching)
        {
            isCrouching = true;
        }
        else if (context.canceled && touchingDirection.isGrounded  && isCrouching)
        {
            isCrouching = false;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

}
