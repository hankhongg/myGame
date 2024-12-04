using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    // Start is called before the first frame update
    public ContactFilter2D castFilter;
    CapsuleCollider2D touchingCol;
    Animator animator;

    public Vector2 wallDirections => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    // raycasthits
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    // distances
    float wallDistance = 0.03f;
    float groundDistance = 0.1f;
    float ceilingDistance = 0.1f;

    [SerializeField]
    private bool _isGrounded = true;
    public bool isGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrouded, value);
        }
    }
    [SerializeField]
    private bool _isOnWall = false;
    public bool isOnWall
    {
        get
        {
            return _isOnWall;
        }
        set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }
    [SerializeField]
    private bool _isOnCeiling = false;
    public bool isOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling= value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }


    void Start()
    {
        
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        isOnWall = touchingCol.Cast(wallDirections, castFilter, wallHits, wallDistance) > 0;
        isOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
