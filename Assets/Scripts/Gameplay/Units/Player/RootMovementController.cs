using JSAM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RootMovementController : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private GameObject rootPrefab;
    [Space]
    public LayerMask environmentMask;
    public LayerMask rootMask;
    [SerializeField] public float maxRootLength;
    [Space][Header("Spring")]
    [SerializeField] private float springForce;
    [SerializeField] private float springDrag;
    [Space][Header("Sway")]
    [SerializeField] private float swayDrag;
    [Space][Header("Pull")]
    [SerializeField] private float pullForceIncrease;
    [SerializeField] private float pullDistDecrease;
    [Space][Header("Move")]
    [SerializeField] private float moveSpeed;
    [Space][Header("Fixed state")]
    [SerializeField] private float fixedStateSpeed;
    [SerializeField] private Vector2 fixedStateDragRange;
    [SerializeField] private float fixedStateTime;
    [SerializeField] private float branchTightSpeed;
    [Space][Header("Grab")]
    public float grabDist;
    [SerializeField] private float grabSpringDrag;
    [SerializeField] private float grabSwayDrag;
    [Space][Header("Throw")]
    [SerializeField] private float throwReadyTime;
    [SerializeField] private AnimationCurve throwCurve;
    [SerializeField] private float throwDistOffset;
    [SerializeField] private float launchDistMult;
    [SerializeField] private float launchGraceTime;
    [SerializeField] private float launchForce;
    [SerializeField] private float aimForce;
     

    [HideInInspector] public List<Root> Roots;
    [HideInInspector] public List<Root> GrabRoots;
    private List<Root> shootRoots = new();
    [SerializeField] public float rootLengthLeft;

    // pulling
    private float pullTimer = 0.0f;
    private float pullStartDist = 0.0f;

    // fixed state
    private float fixedStateTimer = 0.0f;
    private bool fixedState = false;

    private Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D circleCollider;
    private PlayerInput playerInput;
    private PlayerObject _playerObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        _playerObject = GetComponent<PlayerObject>();

        rootLengthLeft = maxRootLength;

        Roots = new List<Root>();
        GrabRoots = new List<Root>();
    }

    private void Update()
    {
        if (_playerObject.UnitStats.IsDead)
        {
            ClearAllRoots();
        }
        // selected root update
        if (playerInput.SelectedType == RootSelectionType.PULL)
        {
            pullTimer += Time.deltaTime;
            if (Roots.Contains(playerInput.SelectedRoot))
            {
                float oldDist = playerInput.SelectedRoot.Distance;
                playerInput.SelectedRoot.Distance = Mathf.Max(pullStartDist - pullDistDecrease * pullTimer, 0.0f);

                AddRootLength(oldDist - playerInput.SelectedRoot.Distance);
            }
            else
            {
                float oldDist = playerInput.SelectedRoot.Distance;
                float distMult = (throwCurve.Evaluate(pullTimer / throwReadyTime) + throwDistOffset) / (1.0f + throwDistOffset);
                playerInput.SelectedRoot.Distance = pullStartDist * distMult;
                AddRootLength(oldDist - playerInput.SelectedRoot.Distance);
            }
        }
        else if (playerInput.SelectedType == RootSelectionType.MOVE)
        {
            Vector2 playerPos = transform.position;
            float movedDist = moveSpeed * Time.deltaTime;
            float oldDist = playerInput.SelectedRoot.Distance;
            playerInput.SelectedRoot.Distance = Mathf.Max(playerInput.SelectedRoot.Distance - movedDist, 0.0f);

            AddRootLength(oldDist - playerInput.SelectedRoot.Distance);

            Vector2 moveOffset = (playerInput.SelectedRoot.ConnectedPoint - playerPos).normalized * movedDist;
            foreach (Root r in Roots)
            {
                if (r == playerInput.SelectedRoot) continue;

                oldDist = (r.ConnectedPoint - playerPos).magnitude;
                float newDist = (r.ConnectedPoint - (playerPos + moveOffset)).magnitude;

                if (AddRootLength(oldDist - newDist))
                {
                    r.Distance += newDist - oldDist;
                }
            }
        }

        // fixed state update
        if (rb.velocity.sqrMagnitude < fixedStateSpeed * fixedStateSpeed && Roots.Count > 1 && playerInput.SelectedType != RootSelectionType.PULL)
        {
            if (!fixedState)
            {
                fixedStateTimer += Time.deltaTime;
                rb.drag = Mathf.Lerp(fixedStateDragRange.x, fixedStateDragRange.y, fixedStateTimer / fixedStateTime);
                if (fixedStateTimer >= fixedStateTime) fixedState = true;
            }
            else
            {
                rb.drag = fixedStateDragRange.y;
                rb.gravityScale = 0.0f;

                // tighten branches
                foreach (Root r in Roots)
                {
                    if (playerInput.SelectedRoot == r) continue;

                    float actualDist = (r.ConnectedPoint - ((Vector2)transform.position)).magnitude;
                    float oldDist = r.Distance;
                    float newDist = Mathf.MoveTowards(r.Distance, actualDist, branchTightSpeed * Time.deltaTime);
                    if (AddRootLength(oldDist - newDist))
                    {
                        r.Distance = newDist;
                    }
                }
            }
        }
        else
        {
            fixedState = false;
            fixedStateTimer = 0.0f;
            rb.drag = fixedStateDragRange.x;
            rb.gravityScale = 1.0f;
        }
    }

    private void FixedUpdate()
    {
        foreach (Root r in Roots)
        {
            // spring force
            Vector2 springDir = (r.ConnectedPoint - ((Vector2)transform.position));
            float dist = springDir.magnitude;
            springDir /= dist;
            float offset = dist - r.Distance;
            // formula for spring force, currently y = x^2
            float springF = offset * offset;
            if (offset < 0.0f) springF *= -1.0f;
            //
            if (r == playerInput.SelectedRoot && playerInput.SelectedType == RootSelectionType.PULL)
                springF *= 1.0f + pullTimer * pullForceIncrease;
            rb.AddForce(rb.mass * springF * springForce * Time.fixedDeltaTime * springDir);

            // spring drag
            ApplyDrag(springDir, springDrag, rb);

            // sway drag
            Vector2 swayDir = Vector2.Perpendicular(((Vector2)transform.position) - r.ConnectedPoint).normalized;
            ApplyDrag(swayDir, swayDrag, rb);
        }

        List<Root> clearedRoots = new();
        foreach (Root r in GrabRoots)
        {
            if (r.GrabBody == null) continue;

            // calc part of force on player
            float playerImpact = rb.gravityScale;
            if (playerInput.SelectedRoot == r || r.Launching) playerImpact = 0.0f;

            float totalMass = r.GrabBody.mass + rb.mass;
            float playerMassShare = Mathf.Lerp(1.0f - (rb.mass / totalMass), 0.0f, playerImpact);
            float objectMassShare = Mathf.Lerp(1.0f - (r.GrabBody.mass / totalMass), 1.0f, playerImpact);

            // spring force
            Vector2 springDir = (((Vector2)transform.position) - r.ConnectedPoint);
            float dist = springDir.magnitude;
            springDir /= dist;
            float offset = dist - r.Distance;
            // formula for spring force, currently y = x^2
            float springF = offset * offset;
            if (offset < 0.0f) springF *= -1.0f;
            //
            if (r.Launching)
            {
                r.GrabBody.AddForce(r.GrabBody.mass * springF * launchForce * Time.fixedDeltaTime * springDir);
            }
            else
            {
                r.GrabBody.AddForce(objectMassShare * r.GrabBody.mass * springF * springForce * Time.fixedDeltaTime * springDir);
                rb.AddForce(playerMassShare * rb.mass * springF * springForce * Time.fixedDeltaTime * -springDir);
            }

            // spring drag
            ApplyDrag(springDir, objectMassShare * grabSpringDrag, r.GrabBody);
            ApplyDrag(springDir, playerMassShare * springDrag, rb);

            Vector2 swayDir = Vector2.Perpendicular(((Vector2)transform.position) - r.ConnectedPoint).normalized;
            // swayDrag
            if (playerInput.SelectedRoot == r)
            {
                ApplyDrag(swayDir, grabSwayDrag, r.GrabBody);
            }

            // aiming
            if (playerInput.SelectedRoot == r)
            {
                float aimOffset = -Vector2.SignedAngle(-springDir, playerInput.AimDir);
                float aimF = aimOffset * aimOffset;
                if (aimOffset < 0.0f) aimF *= -1.0f;
                r.GrabBody.AddForce(r.GrabBody.mass * aimF * aimForce * swayDir, ForceMode2D.Force);
            }

            if (r.Launching)
            {
                // grace timer
                r.GraceTimer -= Time.fixedDeltaTime;

                // maxVel
                Vector2 launchDir = (r.ConnectedPoint - ((Vector2)transform.position)).normalized;
                float newForce = Vector2.Dot(r.GrabBody.velocity, launchDir);

                // stopping
                if (r.GraceTimer < 0.0f && newForce < r.LastForce)
                {
                    clearedRoots.Add(r);
                } 
                else
                {
                    r.LastForce = Mathf.Max(newForce, r.LastForce);
                }
            }
        }

        foreach (Root r in clearedRoots)
        {
            ClearRoot(r);
        }

        _playerObject.ImpulseSource.enabled = !(rb.velocity.magnitude < 15);
    }

    private static void ApplyDrag(Vector2 projectAxis, float dragAmount, Rigidbody2D rigidbody)
    {
        float dragVel = Vector2.Dot(rigidbody.velocity, projectAxis);
        float dragF = Mathf.Min(dragAmount * Time.fixedDeltaTime, 1.0f) * dragVel;
        rigidbody.AddForce(rigidbody.mass * -dragF * projectAxis, ForceMode2D.Impulse);
    }

    public Root ConstructRoot(Vector2 dir)
    {
        GameObject newRoot = Instantiate(rootPrefab, transform.position, Quaternion.identity, transform);
        Root newR = newRoot.GetComponent<Root>();
        newR.Create(dir, this);
        shootRoots.Add(newR);

        AudioManager.PlaySound(Sounds.ShootRoots, transform.position);

        return newR;
    }

    public void RootConnected(Root root, bool isFixed)
    {
        shootRoots.Remove(root);
        if (isFixed)
        {
            Roots.Add(root);
        }
        else
        {
            GrabRoots.Add(root);

            foreach (Root r in GrabRoots)
            {
                if (r == root) continue;

                if (r.GrabBody == root.GrabBody)
                {
                    ClearRoot(root);
                    break;
                }
            }
        }
    }

    public void ClearRoot(Root root)
    {
        root.Retract();

        if (playerInput.SelectedRoot == root)
        {
            playerInput.SelectedRoot = null;
            playerInput.SelectedType = RootSelectionType.NONE;
        }

        if (root.Fixed)
        {
            Roots.Remove(root);
        }
        else
        {
            GrabRoots.Remove(root);
        }

        AudioManager.PlaySound(Sounds.CutRoots, transform.position);
    }

    public void StartPull()
    {
        pullStartDist = playerInput.SelectedRoot.Distance;
        pullTimer = 0.0f;
    }

    public void EndPull()
    {
        if (Roots.Contains(playerInput.SelectedRoot))
        {
            ClearRoot(playerInput.SelectedRoot);
        }
        else
        {
            float newDist = pullStartDist * launchDistMult;
            newDist = Mathf.Min(newDist, rootLengthLeft);
            AddRootLength(playerInput.SelectedRoot.Distance - newDist);
            playerInput.SelectedRoot.Distance = newDist;
            playerInput.SelectedRoot.Launching = true;
            playerInput.SelectedRoot.LastForce = 0.0f;
            playerInput.SelectedRoot.GraceTimer = launchGraceTime;
        }
    }

    public void CancelRoot(Root root)
    {
        shootRoots.Remove(root);
    }

    public void ClearAllRoots()
    {
        int t_rootCount = Roots.Count;
        for (int i = 0; i < t_rootCount; i++)
        {
            ClearRoot(Roots[0]);
        }
        AudioManager.PlaySound(Sounds.CutRoots, transform.position);
    }

    public bool AddRootLength(float length)
    {
        if (-length > rootLengthLeft && length < 0.0f) return false;
        rootLengthLeft += length;
        return true;
    }
}
