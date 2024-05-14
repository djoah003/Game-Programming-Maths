using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public class BezierCurve : MonoBehaviour
{
    public List<Transform> points = new List<Transform>(4);

    public Vector3 GetFirstPoint() => points.First().position;
    public Vector3 GetLastPoint() => points.Last().position;

    public List<Vector3> GetNormals()
    {
        List<Vector3> normals = null;

        for (int i = 1; i < 3; i++)
        {
            normals.Add(points[i].position);
        }

        return normals;
    }
}