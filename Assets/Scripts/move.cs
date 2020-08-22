using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField]
    LayerMask lmWalls;
    [SerializeField]
    float fJumpVelocity = 5;

    Rigidbody2D rb;

    float timeSinceJumpPress = 0;
    [SerializeField]
    float fJumpPressedRememberTime = 0.2f;

    public float timeSinceGrounded = 0;
    [SerializeField]
    float fGroundedRememberTime = 0.25f;

    [SerializeField]
    float fHorizontalAcceleration = 1;
    [SerializeField]
    [Range(0, 1)]
    float damping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenStopping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenTurning = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    float fCutJumpHeight = 0.5f;
    [SerializeField]
    float speed;
    public int dir;
    public bool isWallStick = false;
    public LayerMask lmWallStick;
    float wallJumpColliderLen = 6.4f;
    public float timeSinceNoXvJump = 0;
    float maxTimeSinceNoXvJump = 0.4f;
    public float timeSinceWallStuck = 0;
    float maxTimeSinceWallStuck = 0.4f;
    public bool lastWallStuckStatus = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 v2GroundedBoxCheckPosition = (Vector2)transform.position + new Vector2(0, -0.01f);
        Vector2 v2GroundedBoxCheckScale = (Vector2)transform.localScale + new Vector2(-0.02f, 0);
        bool isGrounded = Physics2D.OverlapBox(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, lmWalls);
        isWallStick = Physics2D.OverlapBox(transform.position, new Vector2(wallJumpColliderLen, 1.5f), 0, lmWallStick);
        timeSinceGrounded -= Time.deltaTime;
        if (isWallStick)
        {
            isGrounded = true;
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 17;
        }
        if (isGrounded)
        {
            timeSinceGrounded = fGroundedRememberTime;
        }
        timeSinceWallStuck -= Time.deltaTime;
        timeSinceNoXvJump -= Time.deltaTime;
        if (timeSinceNoXvJump > 0 && timeSinceNoXvJump < maxTimeSinceNoXvJump - Time.deltaTime * 8)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("Horizontal") < -0.3f && timeSinceWallStuck > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, fJumpVelocity);
                timeSinceNoXvJump = 0;
            }
        }
        timeSinceJumpPress -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            timeSinceJumpPress = fJumpPressedRememberTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0.3f)
        {
            dir = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.3f)
        {
            dir = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if ((timeSinceJumpPress > 0) && (timeSinceGrounded > 0))
        {
            timeSinceJumpPress = 0;
            timeSinceGrounded = 0;
            if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("Horizontal") < -0.3f || !isWallStick)
            {
                rb.velocity = new Vector2(rb.velocity.x, fJumpVelocity);
            }
            else
            {
                ContactPoint2D[] points = new ContactPoint2D[10];
                GetComponent<BoxCollider2D>().GetContacts(points);
                if (points.Length > 0)
                {
                    timeSinceNoXvJump = maxTimeSinceNoXvJump;
                    rb.velocity = new Vector2(points[0].normal.x * 80, rb.velocity.y);
                }
            }
            isWallStick = false;
        }
        if (!Input.GetMouseButton(0) || isGrounded)
        {
            float fHorizontalVelocity = rb.velocity.x / speed;
            fHorizontalVelocity += Input.GetAxisRaw("Horizontal") * Time.deltaTime * 40;
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(fHorizontalVelocity))
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                fHorizontalVelocity *= Mathf.Pow(1f - damping, Time.deltaTime * 10f);
            rb.velocity = new Vector2(fHorizontalVelocity * speed, rb.velocity.y);
        }
        if (isWallStick)
        {
            //rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (isWallStick != lastWallStuckStatus)
        {
            lastWallStuckStatus = isWallStick;
            if (isWallStick)
            {
                timeSinceWallStuck = maxTimeSinceWallStuck;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(wallJumpColliderLen, 1.5f, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

}