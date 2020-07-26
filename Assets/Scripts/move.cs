using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField]
    LayerMask lmWalls;
    [SerializeField]
    float fJumpVelocity = 5;

    Rigidbody2D rigid;

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

    void Start ()
    {
        rigid = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        Vector2 v2GroundedBoxCheckPosition = (Vector2)transform.position + new Vector2(0, -0.01f);
        Vector2 v2GroundedBoxCheckScale = (Vector2)transform.localScale + new Vector2(-0.02f, 0);
        bool bGrounded = Physics2D.OverlapBox(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, lmWalls);

        timeSinceGrounded -= Time.deltaTime;
        if (bGrounded)
        {
            timeSinceGrounded = fGroundedRememberTime;
        }

        timeSinceJumpPress -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            timeSinceJumpPress = fJumpPressedRememberTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (rigid.velocity.y > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * fCutJumpHeight);
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0.3f){
            dir = 1;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.3f){
            dir = -1;
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        if ((timeSinceJumpPress > 0) && (timeSinceGrounded > 0))
        {
            timeSinceJumpPress = 0;
            timeSinceGrounded = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, fJumpVelocity);
        }
        if (!Input.GetMouseButton(0) || bGrounded){
        float fHorizontalVelocity = rigid.velocity.x / speed;
        fHorizontalVelocity += Input.GetAxisRaw("Horizontal") * Time.deltaTime * 40;
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
            fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * 10f);
        else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(fHorizontalVelocity))
            fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * 10f);
        else
            fHorizontalVelocity *= Mathf.Pow(1f - damping, Time.deltaTime * 10f);
        rigid.velocity = new Vector2(fHorizontalVelocity * speed, rigid.velocity.y);
        }
    }
}