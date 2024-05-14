using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DemoRoad : MonoBehaviour
{


    [SerializeField]
    public Mesh2D Roadshape;

    public List<BezierPoint> points = new List<BezierPoint>();

    public bool ClosedPath = false;
    public bool repeatedPoint = false;

    [Range(0f, 1f)]
    public float TSimulate = 0.0f;

    [Range(3, 1000)]
    public int Slices = 32;

    public Mesh mesh;
    
    
    [SerializeField] private GameObject car;
    [SerializeField] private float carSpeed;
    OrientedPoint GetBezierOrientedPoint(float t, Vector3 first_a, Vector3 first_c,Vector3 second_c, Vector3 second_a)
    {
        OrientedPoint op;

        Vector3 a = Vector3.Lerp(first_a, first_c, t);
        Vector3 b = Vector3.Lerp(first_c, second_c, t);
        Vector3 c = Vector3.Lerp(second_c, second_a, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 bez = Vector3.Lerp(d, e, t);

        op.Pos = bez;

        Quaternion rotation = Quaternion.LookRotation(e - d);
        op.Rot = rotation;

        return op;
    }

    private void OnValidate()
    {
        if (mesh == null)
            mesh = new Mesh();
        else
            mesh.Clear();
        
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int n = points.Count;
        int nSegments = n - 1;
        
        if (ClosedPath && !repeatedPoint)
        {
            points.Add(points.First());
            repeatedPoint = true;
        }


        // Loop through the slices
        for (int slice = 0; slice <= Slices; slice++)
        {

            float TSlice = (float)slice / Slices;  // 0.0f ... 1.0f

            int segStart = Mathf.FloorToInt(TSlice * nSegments);
            if (segStart >= nSegments)
            {
                segStart = nSegments - 1;
            }
            
            Vector3 firstAnchor = points[segStart].GetAnchorPoint();
            Vector3 secondAnchor = points[segStart + 1].GetAnchorPoint();
            Vector3 firstControl = points[segStart].GetSecondControlPoint();
            Vector3 secondControl = points[segStart + 1].GetFirstControlPoint();
            
            float TActual = nSegments * TSlice - segStart * 1.0f;

            OrientedPoint op = GetBezierOrientedPoint(TActual, firstAnchor, firstControl, secondControl, secondAnchor);

            for (int i = 0; i < Roadshape.vertices.Length; i++)
            {
                int j = i + 2;
                Vector3 roadPoint = Roadshape.vertices[i].point;
                
                Vector3 transformedPoint = op.LocalToWorldPosition(roadPoint);
                vertices.Add(transformedPoint);
                normals.Add(op.LocalToWorldVector(Roadshape.vertices[i].normal));

                float t = slice / (Slices - 1f);
                uvs.Add(new Vector2(Roadshape.vertices[i].u,t));
            }

            // Triangles
            for (int i = 0; i < Roadshape.vertices.Length - 2; i += 2)
            { 
                if (slice == Slices)
                {
                    break;
                }

                int firstStart = slice * Roadshape.vertices.Length + i + 1;
                int firstEnd = firstStart + 1;

                int secondStart = firstStart + Roadshape.vertices.Length;
                int secondEnd = firstEnd + Roadshape.vertices.Length;

                // 1st triangle
                triangles.Add(firstStart);
                triangles.Add(secondStart);
                triangles.Add(secondEnd);

                // 2nd triangle
                triangles.Add(firstStart);
                triangles.Add(secondEnd);
                triangles.Add(firstEnd);
            }

            if (slice < Slices)
            {
                // Special case, loop around the 2D mesh
                int indexStart = slice * Roadshape.vertices.Length + 15;
                int indexEnd = slice * Roadshape.vertices.Length;

                int nextStart = (slice + 1) * Roadshape.vertices.Length + 15;
                int nextEnd = (slice + 1) * Roadshape.vertices.Length;

                // 1st triangle
                triangles.Add(indexStart);
                triangles.Add(nextStart);
                triangles.Add(nextEnd);

                // 2nd triangle
                triangles.Add(indexStart);
                triangles.Add(nextEnd);
                triangles.Add(indexEnd);
            }
        }
        
        car.transform.position = CalculateBezierPoint(TSimulate, points[0].GetAnchorPoint(), points[0].GetFirstControlPoint(), points[0].GetSecondControlPoint(), points[points.Count - 1].GetAnchorPoint());

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        GetComponent<MeshFilter>().sharedMesh = mesh;   
    }
    
    private void Update()
    {
            TSimulate += Time.deltaTime * carSpeed;
            if(TSimulate >= 1)
                TSimulate = 0;
    
            Vector3 tangent = GetTangentOnPath(TSimulate, points, ClosedPath);
            car.transform.position = GetPointOnPath(TSimulate, points);
            Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up);
            car.transform.rotation = rotation;
    }
    
    private Vector3 CalculateBezierPoint(float t, Vector3 firstA, Vector3 firstC, Vector3 secondC, Vector3 secondA)
    {
        Vector3 a = Vector3.Lerp(firstA, firstC, t);
        Vector3 b = Vector3.Lerp(firstC, secondC, t);
        Vector3 c = Vector3.Lerp(secondC, secondA, t);
    
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
    
        Vector3 bez = Vector3.Lerp(d, e, t);
        return bez;
    }
    

    // Function to get the point on the entire Bezier path given a parameter t between 0 and 1
    private Vector3 GetPointOnPath(float t, List<BezierPoint> points)
    {
        int n = points.Count - 1;

        // Get the total number of segments
        int nSegments = n;

        // Ensure t is between 0 and 1
        t = Mathf.Clamp01(t);

        // Calculate which segment we are on
        float segmentLength = 1f / nSegments;
        int segmentIndex = Mathf.FloorToInt(t / segmentLength);

        // Calculate the local t value within the segment
        float localT = (t - segmentIndex * segmentLength) / segmentLength;

        // Get the corresponding points for the segment
        int p1Index = segmentIndex;
        int p2Index = (segmentIndex + 1) % n;

        BezierPoint p1 = points[p1Index];
        BezierPoint p2 = points[p2Index];

        Vector3 anchor1 = p1.GetAnchorPoint();
        Vector3 control1 = p1.GetSecondControlPoint();
        Vector3 control2 = p2.GetFirstControlPoint();
        Vector3 anchor2 = p2.GetAnchorPoint();

        // Calculate the point on the cubic Bezier curve
        return CalculateBezierPoint(localT, anchor1, control1, control2, anchor2);
    
    }

    // Function to calculate a cubic Bezier tangent
    private Vector3 CalculateBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // Derivative formula for cubic Bezier
        return 3 * Mathf.Pow(1 - t, 2) * (p1 - p0) +
               6 * (1 - t) * t * (p2 - p1) +
               3 * Mathf.Pow(t, 2) * (p3 - p2);
    }

    // Function to get the tangent on the entire Bezier path
    public Vector3 GetTangentOnPath(float t, List<BezierPoint> points, bool closedPath)
    {
        int n = points.Count - 1;

        t = Mathf.Clamp01(t);

        float segmentLength = 1f / n;
        int segmentIndex = Mathf.FloorToInt(t / segmentLength);

        float localT = (t - segmentIndex * segmentLength) / segmentLength;

        int p1Index = segmentIndex;
        int p2Index = (segmentIndex + 1) % n;

        BezierPoint p1 = points[p1Index];
        BezierPoint p2 = points[p2Index];

        Vector3 anchor1 = p1.GetAnchorPoint();
        Vector3 control1 = p1.GetSecondControlPoint();
        Vector3 control2 = p2.GetFirstControlPoint();
        Vector3 anchor2 = p2.GetAnchorPoint();

        // Calculate the tangent on the cubic Bezier curve
        return CalculateBezierTangent(localT, anchor1, control1, control2, anchor2);
    }
}