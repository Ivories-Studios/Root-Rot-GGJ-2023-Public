using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class RootGraphics : MonoBehaviour
{
    [HideInInspector] public Vector3 Endpoint;
    [HideInInspector] public float Distance;

    public int rootCount;
    [SerializeField] private float rootSpace;
    [SerializeField] private float rootRadius;
    [SerializeField] private float rootTurn;
    [Space]
    [SerializeField] private AnimationCurve rootSize;
    [SerializeField] private AnimationCurve anchorInfluence;
    [SerializeField] private float anchorLength;
    [SerializeField] private float anchorDepth;
    [SerializeField] private float anchorSize;
    [SerializeField] private AnimationCurve anchorNormalInfluence;
    [Space]
    [SerializeField] private AnimationCurve speedBendCurve;
    [SerializeField] private Vector2 speedStrength;
    [SerializeField] private float branchTension;
    [SerializeField] private float branchDrag;
    [Space]
    [Header("Quality")]
    [SerializeField] private int sides;
    [SerializeField] private int segments;
    [SerializeField] private int subdivisions;
    [SerializeField] private float tangentSize;
    [Space]
    [Header("Safety")]
    [SerializeField] private Vector2 stretchRange;
    [SerializeField] private float minLength;


    // physics
    private Vector2 branchVel = Vector2.zero;
    private Vector2 branchPos = Vector2.zero;
    private Vector2 lastBranchPos;

    //private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private MeshFilter filter;

    private bool anchored = false;
    private Vector2 surfaceNormal = Vector2.zero;

    private void Awake()
    {
        //lineRenderer= GetComponent<LineRenderer>();
        edgeCollider= GetComponent<EdgeCollider2D>();
        filter= GetComponent<MeshFilter>();

        lastBranchPos = transform.position;
    }

    public void UpdateGraphics()
    {

        Vector3 cPoint = transform.InverseTransformPoint(Endpoint);
        edgeCollider.SetPoints(new List<Vector2>()
            {
                Vector2.zero,
                cPoint
            });


        CombineInstance[] combine = new CombineInstance[rootCount];

        Vector3 up = Endpoint - transform.position;
        float branchLength = up.magnitude;
        up /= branchLength;
        Vector3 right = Vector3.Cross(up, Vector3.forward);
        float stretch = Mathf.Clamp(Distance / branchLength, stretchRange.x, stretchRange.y);

        if (branchLength < minLength)
        {
            filter.sharedMesh = new Mesh();
            return;
        }

        for (int i = 0; i < rootCount; i++) 
        {
            Mesh m = new();
            Spline spline = new();
            spline.SetTangentMode(TangentMode.AutoSmooth);

            float branchRotation = (i / ((float)rootCount)) * 360.0f;
            int points = subdivisions + 2;
            for (int j = 0; j < points; j++)
            {
                // use eval function
                float alongBranch = j / (points - 1.0f);
                BezierKnot knot = EvalBranch(alongBranch, branchLength, stretch, up, right, branchRotation);

                spline.Add(knot);
            }

            // in wall point
            BezierKnot wallKnot = EvalBranch(1.0f, branchLength, stretch, up, right, branchRotation);
            wallKnot.Position += (float3)(-((Vector3)surfaceNormal) * anchorDepth);
            spline.Add(wallKnot);

            // create mesh
            SplineMesh.Extrude(spline, m, rootRadius, sides, (int)(segments * branchLength));
            combine[i].mesh = m;
            combine[i].transform = Matrix4x4.identity;

            // prevent memory leak
            m = null;
        }
        Mesh mesh = new();
        mesh.CombineMeshes(combine);
        filter.sharedMesh = mesh;
        mesh = null;
    }

    public BezierKnot EvalBranch(float alongBranch, float length, float stretch, Vector3 up, Vector3 right, float branchRotation)
    {
        Vector3 point = Vector3.zero;
        
        if (anchored && anchorLength > 0.0f)
        {
            float anchorD = (1.0f - alongBranch) * length;
            float anchorX = anchorD / anchorLength;
            Vector3 addition = Vector3.Lerp(
                alongBranch * length * up,
                ((Vector3)surfaceNormal) * anchorD + (Endpoint - transform.position),
                anchorNormalInfluence.Evaluate(anchorX)
                );
            point += addition;

            up = Vector3.Lerp(up, -surfaceNormal, anchorNormalInfluence.Evaluate(anchorX)).normalized;
            right = Vector3.Cross(up, Vector3.forward);
        }
        else
        {
            point += alongBranch * length * up;
        }
        Vector3 rotPoint = point;

        float size = Mathf.Clamp(stretch, 0.1f, 2.0f) * rootSize.Evaluate(alongBranch);
        if (anchored && anchorLength > 0.0f)
        {
            float anchorDist = (1.0f - alongBranch) * length;
            anchorDist /= anchorLength;
            size += anchorInfluence.Evaluate(anchorDist) * anchorSize;
        }
        point += size * rootSpace * right;
        
        point = Quaternion.AngleAxis(rootTurn * (1.0f - alongBranch) * length * stretch + branchRotation, up) * (point - rotPoint) + rotPoint;

        // branch speed
        float speedInfluence = speedBendCurve.Evaluate(alongBranch);
        Vector2 vel = Quaternion.FromToRotation(up, Vector2.up) * branchPos;
        point += speedStrength.y * length * speedInfluence * vel.y * up 
            + speedStrength.x * length * speedInfluence * vel.x * right;

        Vector3 tangent = new(0.0f, 0.0f, ((length / stretch) * tangentSize) / (subdivisions + 2.0f));
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, -up) * Quaternion.AngleAxis(branchRotation, up);
        return new BezierKnot(point, -tangent, tangent, rotation);
    }

    public void Anchor(Vector2 normal)
    {
        anchored= true;
        surfaceNormal = normal;
    }

    public void Deanchor()
    {
        anchored= false;
    }

    private void FixedUpdate()
    {
        // drag
        branchVel -=  branchVel * Mathf.Min(branchDrag * Time.fixedDeltaTime, 1.0f);

        // speed
        Vector2 branchP = (Endpoint + transform.position) / 2.0f;
        branchVel += branchP - lastBranchPos;
        lastBranchPos = branchP;

        // tension
        Vector2 tension = -branchPos * branchTension  * new Vector2(branchPos.x * branchPos.x, branchPos.y * branchPos.y);
        branchVel += tension * Time.fixedDeltaTime;

        // position update
        branchPos += branchVel * Time.fixedDeltaTime;
    }
}
