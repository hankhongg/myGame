using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 5;
    public float walkStopRate = 0.5f;

   public Rigidbody2D rb;
    public TouchingDirection touchingDirection;
    public DetectionZone attackZone;
   public Animator animator;

    public enum walkingDirectionLR { Left, Right };
    private walkingDirectionLR walkDirection;
    private Vector2 walkDirectionVector;


    private bool hasTarget;
    public bool HasTarget { get { return hasTarget; } set { hasTarget = value; animator.SetBool(AnimationStrings.hasTarget, value); } }
    private bool canMove;
    public bool CanMove { get { return animator.GetBool(AnimationStrings.canMove); } }
    public walkingDirectionLR WalkDirection
    {
        get
        {
            return walkDirection;
        }
        set
        {
            if (walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == walkingDirectionLR.Right)
                {
                    walkDirectionVector = Vector2.left;
                }
                else if (value == walkingDirectionLR.Left)
                {
                    walkDirectionVector = Vector2.right;
                }
                walkDirection = value;
            }       
        }
    }

    private void Update()
    {
        HasTarget = attackZone.colliders.Count > 0;
    }

    private void Awake()
    {
        touchingDirection = GetComponent<TouchingDirection>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (touchingDirection.isOnWall && touchingDirection.isGrounded)
        {
            Flip();
        }
        //rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        if (CanMove)
        {
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        }
        else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
    }


    private void Flip()
    {
        if (WalkDirection == walkingDirectionLR.Left)
        {
            //walkDirectionVector = Vector2.right;
            WalkDirection = walkingDirectionLR.Right;
        }
        else if (WalkDirection == walkingDirectionLR.Right)
        {
            //walkDirectionVector = Vector2.left;
            WalkDirection = walkingDirectionLR.Left;
        }
    }
}
