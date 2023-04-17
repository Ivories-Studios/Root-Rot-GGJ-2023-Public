using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [HideInInspector] public Vector2 ConnectedPoint;
    [HideInInspector] public float Distance;
    [HideInInspector] public bool Fixed;
    [HideInInspector] public Rigidbody2D GrabBody;
    [HideInInspector] public Vector2 GrabOffset;
    [HideInInspector] public bool Launching = false;
    [HideInInspector] public float LastForce;
    [HideInInspector] public float GraceTimer;

    [HideInInspector] public EdgeCollider2D EdgeCollider;

    [SerializeField] private float shootSpeed;
    [SerializeField] private float retractDist;
    [SerializeField] private Color highlightColor;
    [SerializeField] private float anchorRadius;

    private RootMovementController con;
    private RootGraphics graphics;
    private Material mat;

    private Color normalCol;
    private bool highlighted = false;

    private RootState state;
    private Vector2 endPoint;
    private Vector2 direction;

    private void Awake()
    {
        EdgeCollider= GetComponent<EdgeCollider2D>();
        graphics= GetComponent<RootGraphics>();

        mat = GetComponent<MeshRenderer>().material;
        normalCol = mat.GetColor("_OutlineColor");

        state = RootState.EXTENDING;
    }

    public void Create(Vector2 dir, RootMovementController con)
    {
        direction = dir;

        endPoint = direction * retractDist + ((Vector2)transform.position);

        graphics.Endpoint = endPoint;
        graphics.UpdateGraphics();

        this.con = con;

        Distance = retractDist;
        if (!con.AddRootLength(-Distance))
        {
            con.CancelRoot(this);
            Retract();
        }
    }

    public void Retract()
    {
        if (state == RootState.EXTENDING)
        {
            con.CancelRoot(this);
        }
        else if (state == RootState.ANCHORED) 
        {
            if (GrabBody != null && GrabBody.TryGetComponent(out UnitObject unit))
            {
                unit.UnitStats.IsGrabbed = false;
            }
        }

        if (Fixed)
        {
            graphics.Deanchor();
        }

        con.AddRootLength(Distance);

        state = RootState.RETRACTING;
    }

    private void LateUpdate()
    {
        if (state == RootState.EXTENDING) 
        {
            endPoint += Time.deltaTime * shootSpeed * direction;

            Vector2 dir = (endPoint - ((Vector2)transform.position));
            float length = dir.magnitude;
            dir /= length;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, length, con.environmentMask);
            if (hit.collider != null)
            {
                float newDist = hit.distance;
                Rigidbody2D rb = hit.transform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    if (rb.bodyType != RigidbodyType2D.Dynamic)
                    {
                        rb = null;
                    }
                    else
                    {
                        newDist = con.grabDist;
                    }
                }

                if (!con.AddRootLength(Distance - newDist))
                {
                    Retract();
                }
                else
                {
                    if (rb != null)
                    {
                        Fixed = false;
                        GrabBody = rb;
                        GrabOffset = hit.collider.offset;
                        ConnectedPoint = hit.transform.position + (GrabBody.transform.rotation * (Vector3)GrabOffset);
                        if (hit.collider.TryGetComponent(out UnitObject unitObject))
                        {
                            unitObject.UnitStats.IsGrabbed = true;
                        }
                    }
                    else
                    {
                        Fixed = true;
                        ConnectedPoint = hit.point;
                        graphics.Anchor(hit.normal);
                    }

                    endPoint = ConnectedPoint;
                    Distance = newDist;
                    state = RootState.ANCHORED;
                    con.RootConnected(this, rb == null);
                }
            }
            else
            {
                if (!con.AddRootLength(Distance - length))
                {
                    Retract();
                }
                Distance = length;
            }
        }
        else if (state == RootState.ANCHORED)
        {
            if (!Fixed) 
            {
                if (GrabBody == null)
                {
                    con.ClearRoot(this);
                    return;
                }
                ConnectedPoint = GrabBody.transform.position + (GrabBody.transform.rotation * (Vector3)GrabOffset);   
            }
            endPoint = ConnectedPoint;
        }
        else
        {
            endPoint = Vector2.MoveTowards(endPoint, transform.position, Time.deltaTime * shootSpeed);

            float dist = (endPoint - ((Vector2)transform.position)).magnitude;
            if (dist < retractDist) Destroy(gameObject);
        }

        graphics.Endpoint = endPoint;
        graphics.Distance = Distance;
        graphics.UpdateGraphics();

        if (highlighted) mat.SetColor("_OutlineColor", highlightColor);
        else mat.SetColor("_OutlineColor", normalCol);
    }

    public void Highlight()
    {
        highlighted= true;
    }

    public void Unhighlight()
    {
        highlighted= false;
    }

    public void TakeDamage(int value)
    {
        graphics.rootCount -= value;
        if (graphics.rootCount <= 0)
        {
            con.ClearRoot(this);
        }
    }

    public bool OverlapPoint(Vector2 point)
    {
        return EdgeCollider.OverlapPoint(point) || (
            state == RootState.ANCHORED 
            && (point - ConnectedPoint).sqrMagnitude < anchorRadius * anchorRadius
            );
    }
}


enum RootState
{
    ANCHORED,
    EXTENDING,
    RETRACTING
}