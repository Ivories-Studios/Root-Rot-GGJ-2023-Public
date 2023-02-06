using UnityEngine;

public class FlyingEnemyAI : AIEnemy
{
    public Vector2 PatrolLocation;
    [Space]
    [SerializeField] private bool avoidance;
    [SerializeField] private LayerMask AvoidanceMask;
    [SerializeField] private float AvoidanceDist;
    [SerializeField] private float AvoidanceRange;
    [SerializeField] private int AvoidanceRays;
    [SerializeField] private float AvoidanceRadius;

    bool foundTarget = false;
    bool movingTowardsPatrol = true;

    float timeToChange = 0;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (target.position - transform.position).normalized, VisionRange, 1<<LayerMask.NameToLayer("Player"));

        if (hit.collider != null)
        {
            foundTarget = true;
        }


        timeToChange -= Time.fixedDeltaTime;
        if (timeToChange < 0)
        {
            if (Vector2.Distance(transform.position, spawnLocation + PatrolLocation) < 0.2)
            {
                movingTowardsPatrol = false;
                timeToChange = 0.1f;
            }

            if (Vector2.Distance(transform.position, spawnLocation) < 0.2)
            {
                movingTowardsPatrol = true;
                timeToChange = 0.1f;
            }

        }


    }

    protected override Vector2 GetDirectionOfMovement()
    {
        Vector2 dir = Vector2.zero;

        if (foundTarget)
        {
            dir = target.position - transform.position;
        }
        else if (movingTowardsPatrol)
        {
            dir = spawnLocation + PatrolLocation - (Vector2)transform.position;

        }
        else if (!movingTowardsPatrol)
        {
            dir = spawnLocation - (Vector2)transform.position;
        }

        if (avoidance)
            return ObjectAvoidance(dir.normalized);
        else 
            return dir.normalized;
    }

    private Vector2 ObjectAvoidance(Vector2 dir)
    {
        float maxDist = 0.0f;
        Vector2 bestDir = Vector2.zero;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, AvoidanceRadius, dir, AvoidanceDist, AvoidanceMask);
        if (hit.collider == null)
        {
            return dir;
        }
        else if (hit.distance > maxDist)
        {
            maxDist = hit.distance;
            bestDir = dir;
        }

        float rot = AvoidanceRange / AvoidanceRays;
        for (int i = 1; i <= AvoidanceRays; i++)
        {
            Vector2 avDir = Quaternion.Euler(0.0f, 0.0f, rot * i) * dir;
            hit = Physics2D.CircleCast(transform.position, AvoidanceRadius, avDir, AvoidanceDist, AvoidanceMask);
            if (hit.collider == null)
            {
                return avDir;
            }
            else if (hit.distance > maxDist)
            {
                maxDist = hit.distance;
                bestDir = avDir;
            }

            avDir = Quaternion.Euler(0.0f, 0.0f, -rot * i) * dir;
            hit = Physics2D.CircleCast(transform.position, AvoidanceRadius, avDir, AvoidanceDist, AvoidanceMask);
            if (hit.collider == null)
            {
                return avDir;
            }
            else if (hit.distance > maxDist)
            {
                maxDist = hit.distance;
                bestDir = avDir;
            }
        }

        return bestDir;
    }
}
