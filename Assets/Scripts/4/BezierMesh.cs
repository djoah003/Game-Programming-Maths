using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class BezierMesh : MonoBehaviour
{

    [Range(3, 100)] public int segmentAmount = 3;
    [Range(1, 100)] public float _thickness;
    List<Vector3> pointsInner = new List<Vector3>();
    List<Vector3> pointsOuter = new List<Vector3>();
    List<Vector3> tri = new List<Vector3>();

    [Range(1, 100)] public float radiusInner;
    private float radiusOuter => radiusInner + _thickness;

    void GenerateMesh()
    {
        Vector3[] newVertices = new Vector3[4];
        newVertices[0] = new Vector3(0, 0, 0);
        newVertices[1] = new Vector3(1, 0, 0);
        newVertices[2] = new Vector3(0.5f, 1, 0);
        newVertices[3] = new Vector3(1.5f, 1, 0);

        int[] newTriangles = new int[6];
        // Fist triangle
        newTriangles[0] = 2;
        newTriangles[1] = 1;
        newTriangles[2] = 0;
        // Second triangle
        newTriangles[3] = 2;
        newTriangles[4] = 3;
        newTriangles[5] = 1;

        // create mesh
        Mesh mesh = new Mesh();
        mesh.SetVertices(newVertices);
        mesh.SetTriangles(newTriangles, 0);

        // get mesh filter and set mesh
        GetComponent<MeshFilter>().sharedMesh = mesh;

    }

    // Start is called before the first frame update
    private void OnValidate()
    {
        // GenerateMesh();

        // GenerateDisc();
    }

    private void OnDrawGizmos()
    {




        DrawWireCircle(pointsInner, transform.position, transform.rotation, radiusInner, segmentAmount);
        DrawWireCircle(pointsOuter, transform.position, transform.rotation, radiusOuter, segmentAmount);
        Debug.Log(pointsInner.Count);
        for (int i = 0; i < pointsInner.Count - 1; i++)
        {
            Gizmos.DrawLine(pointsInner[i], pointsOuter[i + 1]);
        }
/*
        // MESH
        for (int i = 0; i < segmentAmount; i++)
        {
            Gizmos.DrawLine(pointsInner[i], pointsOuter[i]);
        }

        for (int i = 0; i < segmentAmount; i++)
        {
            if (i + 1 < pointsOuter.Count)
            {
                Gizmos.DrawLine(pointsInner[i], pointsOuter[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(pointsInner.Last(), pointsOuter.First());
            }
        }
*/
        static void DrawWireCircle(List<Vector3> points, Vector3 pos, Quaternion rot, float radius, int detail)
        {
            points = new List<Vector3>(detail);
            // POINTS
            for (int i = 0; i < detail; i++)
            {
                float t = i / (float)detail;
                float angRad = t * 2 * Mathf.PI;

                Vector2 point2D = new Vector2(
                    Mathf.Cos(angRad) * radius,
                    Mathf.Sin(angRad) * radius
                );
                points.Add(pos + rot * point2D);
            }

            for (int i = 0; i < detail - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
                Gizmos.DrawSphere(points[i], 1f);
            }

            Gizmos.DrawSphere(points.Last(), 1f);
            Gizmos.DrawLine(points.Last(), points.First());
        }
    }
}
