using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyAI : AIEnemy
{
    /// <summary>
    /// Maximum jump height
    /// </summary>
    public float fallHeight;

    /// <summary>
    /// Maximum jump range, ie 2 tiles gap between
    /// </summary>
    public float jumpingRange;


    public float WalkUpHeight = 0.2f;
    public float JumpDownHeight = 0.7f;
    public float JumpSpeed = 10;

    public float overlapSize = 0.5f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _obstacleMask;
    float jumpCooldown = 0;

    public bool showDebugLines = false;

    bool isGrounded;

    protected override void Start()
    {
        base.Start();
        topLeft = transform.GetChild(0);
        topRight = transform.GetChild(1);
        botLeft = transform.GetChild(2);
        botRight = transform.GetChild(3);
        jumpPOV = transform.GetChild(4);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        jumpCooldown -= Time.fixedDeltaTime;

        if (CheckForAvailableJump() && jumpCooldown < 0)
        {
            Jump();
            jumpCooldown = 5f;
        }
        
        if (!CheckForAvailableMovement()) lookingRight = !lookingRight;

        RaycastHit2D hit = Physics2D.Raycast(botRight.position, Vector2.down, 0.1f, _groundMask);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool("Jumping", !isGrounded);

    }


    void Jump()
    {
        animator.SetBool("Jumping", true);
        rb.velocity+=(Vector2.up * JumpSpeed);
    }

    protected override Vector2 GetDirectionOfMovement()
    {
        if (lookingRight) return Vector2.right;
        else return Vector2.left;
    }

    protected override bool CheckForAvailableMovement()
    {
        Transform raycastStart = topLeft;
        bool CanWalk = false;

        RaycastHit2D hit = Physics2D.Raycast(raycastStart.position, ((Vector2.down + GetDirectionOfMovement() / 10).normalized), 1000, _obstacleMask);

        if (showDebugLines)
        {
            testLine.SetPosition(0, raycastStart.position);
            testLine.SetPosition(1, (Vector2)raycastStart.position + ((Vector2.down + GetDirectionOfMovement() / 10).normalized) * 50);
            print(hit.collider.name);
        }

        if (hit.collider != null)
        {
            //print(hit.collider.name);
            if (hit.point.y - botLeft.transform.position.y < 0)
            {
                if (hit.point.y - botLeft.transform.position.y > - fallHeight)
                {
                    if (showDebugLines) print("here");
                    CanWalk = true;
                }

                if (followingPlayer)
                {
                    if (Mathf.Abs(hit.point.y - target.position.y) <
                        Mathf.Abs(transform.position.y - target.position.y) + fallHeight)
                    {
                        CanWalk = true;
                    }
                }
            }
            else
            {
                if (hit.point.y - botLeft.transform.position.y < WalkUpHeight)
                {
                    if (showDebugLines) print("here2");
                    return true;
                }
            }
        }

        if (jumpCooldown > 3.5f)
        {
            return true;
        }

        return CanWalk;
    }

    bool CheckForAvailableJump()
    {
        //Who doesn't love hardcoded values?
        //Jokes aside, in the best case these should be calculated depending on the speed and jump height
        Vector2 diff = Vector2.zero;
        if(!lookingRight) diff = new Vector2(-2.93f, -2.21f);
        if( lookingRight) diff = new Vector2( 2.93f, -2.21f);  


        //testLine.SetPosition(0, jumpPOV.position);
        //testLine.SetPosition(1, (Vector2)jumpPOV.position + diff.normalized * 10);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)jumpPOV.position, diff.normalized * 10, 10, _groundMask);

        //print(Mathf.Abs(hit.point.y - target.position.y) + fallHeight + " < " + Mathf.Abs(botLeft.position.y - target.position.y) );
        if (hit.point.y - botLeft.position.y > 0)
        {
            if(followingPlayer)
                if (Mathf.Abs(hit.point.y - target.position.y) + fallHeight < Mathf.Abs(botLeft.position.y - target.position.y))
                {
                    return true;
                }
        }

        return false;
    }

}
