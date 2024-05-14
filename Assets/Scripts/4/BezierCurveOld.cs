using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierCurveOld : MonoBehaviour
{
    private Vector3 parent, A, B, C, D,
                            O,
                            R, S, X, Y, Z;
    [Range(0f, 1f)] public float t;
    private void OnDrawGizmos()
    {
        parent = transform.position;
        // Points
        A = GameObject.Find("A").transform.position;
        B = GameObject.Find("B").transform.position;
        C = GameObject.Find("C").transform.position;
        D = GameObject.Find("D").transform.position;
        // Draw the points
        DrawPoint(A, 0.33f, Color.blue);
        DrawPoint(B, 0.33f, Color.blue);
        DrawPoint(C, 0.33f, Color.blue);
        DrawPoint(D, 0.33f, Color.blue);
        DrawPoint(O, 0.33f, Color.black);
        DrawPoint(R, 0.33f, Color.blue);
        DrawPoint(S, 0.33f, Color.blue);
        DrawPoint(X, 0.33f, Color.red);
        DrawPoint(Y, 0.33f, Color.red);
        DrawPoint(Z, 0.33f, Color.red);
        // Draw the lines between points.
        Gizmos.color = Color.white;
        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(B, C);
        Gizmos.DrawLine(C, D);
        Gizmos.DrawLine(X, Y);
        Gizmos.DrawLine(Y, Z);
        Gizmos.DrawLine(R, S);
        // Calculate the position using lerp
        X = Vector3.Lerp(A, B, t);
        Y = Vector3.Lerp(B, C, t);
        Z = Vector3.Lerp(C, D, t);
        R = Vector3.Lerp(X, Y, t);
        S = Vector3.Lerp(Y, Z, t);
        O = Vector3.Lerp(R, S, t);
        Handles.DrawBezier(A, D, B, C, Color.green, null,1f);

    }

    static void DrawPoint(Vector3 pos, float radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pos, radius);
    }
}
