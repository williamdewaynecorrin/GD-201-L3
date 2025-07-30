using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 0.1f;
    [SerializeField]
    private float jumpStrength = 1.0f;
    [SerializeField]
    private CircleCollider2D circle;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float minCastDistance = 0.1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float circleGroundedRatio = 0.25f;
    [Range(0.0001f, 1.0f)]
    [SerializeField]
    private float gravity = 0.1f;

    [Header("Animation")]
    [SerializeField]
    private SpriteAnimator animator;
    [SerializeField]
    private string idleAnimationName = "animation_slime_idle";
    [SerializeField]
    private string walkRightAnimationName = "animation_slime_move_right";
    [SerializeField]
    private string walkLeftAnimationName = "animation_slime_move_left";

    private Vector2 frameMovement = Vector2.zero;
    private bool isGrounded = false;
    private float yVelocity = 0.0f;
    private bool jumped = false;
    private Vector3 lastGroundHitPoint;

    void Start()
    {
        
    }

    void Update()
    {
        frameMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            frameMovement += Vector2.right;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            frameMovement -= Vector2.right;
        }

        if(isGrounded)
        {
            if(!jumped && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        if (frameMovement != Vector2.zero)
        {
            frameMovement.Normalize();

            if (frameMovement.x > 0.0f)
                animator.PlayAnimation(walkRightAnimationName, false);
            else
                animator.PlayAnimation(walkLeftAnimationName, false);
        }
        else
        {
            animator.PlayAnimation(idleAnimationName, false);
        }
    }

    private void FixedUpdate()
    {
        // -- grounded check
        bool wasGrounded = isGrounded;
        float castDist = GetCastDistance();
        RaycastHit2D groundHit = default(RaycastHit2D);
        if(yVelocity >= 0.0f)
        {
            groundHit = Physics2D.CircleCast(circle.bounds.center, circle.radius * transform.localScale.x, Vector2.down, castDist, groundMask);
        }

        if (groundHit.collider != null)
            lastGroundHitPoint = groundHit.point;

        if (groundHit.collider != null && GetCircleBottom().y >= groundHit.point.y)
        {
            yVelocity = 0.0f;
            isGrounded = true;
            jumped = false;

            if(!wasGrounded)
            {
                OnGroundLand(groundHit);
            }
        }
        else
        {
            isGrounded = false;
            yVelocity += gravity;
            transform.position += Vector3.down * yVelocity;

            if(wasGrounded)
            {
                OnGroundLeave();
            }
        }

        Vector3 move = frameMovement * moveSpeed;
        transform.position += move;
    }

    private void Jump()
    {
        yVelocity = -jumpStrength;
        jumped = true;
    }

    private void OnGroundLand(RaycastHit2D hit)
    {
        float groundedOffset = Physics2D.defaultContactOffset;
        transform.position = hit.point + Vector2.up * ((circle.radius * transform.localScale.x) - circle.offset.y + groundedOffset);
    }

    private void OnGroundLeave()
    {

    }

    private float GetCastDistance()
    {
        return Mathf.Max(minCastDistance, yVelocity);
    }

    private Vector3 GetCircleBottom()
    {
        return circle.bounds.center + Vector3.down * circle.radius * circleGroundedRatio;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 35, 200, 30), string.Format("Grounded: {0}", isGrounded));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float castDist = GetCastDistance();
        Gizmos.DrawLine(circle.bounds.center + Vector3.down * circle.radius, circle.bounds.center + Vector3.down * circle.radius + Vector3.down * castDist);

        Gizmos.DrawSphere(GetCircleBottom(), 0.05f); 

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(lastGroundHitPoint, 0.075f);
    }
}
