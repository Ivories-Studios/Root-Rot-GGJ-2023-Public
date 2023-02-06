using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{

    public float rangeToTarget = 1;

    public float maxSpeed = 1;
    public float acceleration = 1;
    public bool followingPlayer = true;
    /// <summary>
    /// Range in x and y coordinates to follow until going back, not implemented yet
    /// </summary>
    public Vector2 range;


    public float VisionAngle = 30;
    public float VisionRange = 5;

    public Transform target;

    protected Transform topLeft;
    protected Transform topRight;
    protected Transform botLeft;
    protected Transform botRight;
    protected Transform jumpPOV;

    protected Animator animator;
    protected Vector2 spawnLocation;
    protected bool lookingRight = true;
    protected Rigidbody2D rb;
    protected LineRenderer testLine;

    protected float testTimer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spawnLocation = transform.position; 
        rb = GetComponent<Rigidbody2D>();

        //TODO: Make this an interface, and use it as an interface instead
        testLine = GetComponent<LineRenderer>();
        if (!GlobalVariables.ShowTestingRays)
        {
            Destroy(testLine.gameObject);
        }
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        animator.SetBool("Moving", true);
    }

    

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        testTimer += Time.fixedDeltaTime;
        //print(name+ ": " + Vector2.Dot(GetDirectionOfMovement(), rb.velocity) / GetDirectionOfMovement().magnitude + "<"+ maxSpeed);

        if (Vector2.Distance(transform.position, target.position) > rangeToTarget)
        {
            Vector2 dir = GetDirectionOfMovement();
            if (Vector2.Dot(dir, rb.velocity) / dir.magnitude < maxSpeed)
            {
                transform.localScale = dir.x < 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                float opposingForce = (-Vector2.Dot(rb.velocity, dir)).Remap(-1, 1, 1, 3);
                rb.AddForce(dir * (acceleration * Time.fixedDeltaTime * opposingForce));
            }
        }
        else
        {
            Vector2 dir = GetDirectionOfMovement();
            if (Vector2.Dot(-dir, rb.velocity) / dir.magnitude < maxSpeed)
            {
                float opposingForce = (-Vector2.Dot(rb.velocity, dir)).Remap(-1, 1, 1, 3);
                rb.AddForce(-dir * (acceleration * Time.fixedDeltaTime * opposingForce));
            }
        }

        animator.SetBool("Moving", !(rb.velocity.magnitude < 0.2));
    }


    protected virtual Vector2 GetDirectionOfMovement()
    {
        return Vector2.right;
    }
    protected virtual bool CheckForAvailableMovement()
    {
        return true;
    }

}
