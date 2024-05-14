using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class BezierPath : MonoBehaviour
{
    public Mesh2D shape2D;
    public List<BezierGeometry> points = new List<BezierGeometry>();
    public bool closedPath = false;
    public bool drawDebug = false;
    public GameObject movable;
    
    [Range(0, 1000)]
    public int segments = 0;
    [Range(0, 1000)]
    public int currentSegment = 0;
    


    private void OnValidate()
    {
        if (currentSegment > segments)
        {
            currentSegment = Mathf.Clamp(currentSegment, 0, segments);
        }
    }

  private void OnDrawGizmos()
{
    var position = movable.transform.position;

    for (int j = 0; j < shape2D.vertices.Length - 1; j++)
    {
        Gizmos.DrawSphere(shape2D.vertices[j].point + position, HandleUtility.GetHandleSize(shape2D.vertices[j].point) * 0.1f);
        Gizmos.DrawLine(shape2D.vertices[j].point + position, shape2D.vertices[j+1].point + position);
    }
    Gizmos.DrawSphere(shape2D.vertices.Last().point  + position, HandleUtility.GetHandleSize(shape2D.vertices.Last().point) * 0.1f);
    Gizmos.DrawLine(shape2D.vertices.Last().point + position, shape2D.vertices.First().point + position);
    
    for (int i = 0; i < points.Count; i++) 
    {
        if (i + 1 < points.Count)
        {
            Handles.DrawBezier(points[i].GetAnchorPoint(), 
                points[i + 1].GetAnchorPoint(),
                points[i].GetSecondControlPoint(),
                points[i + 1].GetFirstControlPoint(),
                Color.green, null, 2f);
        }
        
   

        if (closedPath && i == points.Count - 1)
        {
            Handles.DrawBezier(
                points.Last().GetAnchorPoint(),
                points.First().GetAnchorPoint(),
                points.Last().GetSecondControlPoint(),
                points.First().GetFirstControlPoint(),
                Color.green, null, 2f);

            int numberOfSegments = points.Count;
            int segmentsPerPart = segments / numberOfSegments;
            int segmentStart = Mathf.Min(currentSegment / segmentsPerPart, points.Count - 1);

            float tValue = (currentSegment % segmentsPerPart) / (float)segmentsPerPart;

            Vector3 firstAnchor = points[segmentStart].GetAnchorPoint();
            Vector3 secondAnchor = points[(segmentStart + 1) % points.Count].GetAnchorPoint();
            Vector3 firstControl = points[segmentStart].GetSecondControlPoint();
            Vector3 secondControl = points[(segmentStart + 1) % points.Count].GetFirstControlPoint();

            Vector3 a = Vector3.Lerp(firstAnchor, firstControl, tValue);
            Vector3 b = Vector3.Lerp(firstControl, secondControl, tValue);
            Vector3 c = Vector3.Lerp(secondControl, secondAnchor, tValue);
            Vector3 d = Vector3.Lerp(a, b, tValue);
            Vector3 e = Vector3.Lerp(b, c, tValue);
            Vector3 bez = Vector3.Lerp(d, e, tValue);

            Gizmos.DrawSphere(bez, HandleUtility.GetHandleSize(bez) * 0.01f);
            Vector3 forwardVector = (e - d).normalized;
            movable.transform.position = bez;
            movable.transform.forward = forwardVector;
        }
        else if (!closedPath)
        {
            int numberOfSegments = points.Count - 1;
            int segmentsPerPart = segments / numberOfSegments;
            int segmentStart = Mathf.Min(currentSegment / segmentsPerPart, points.Count - 1);

            float tValue = (currentSegment % segmentsPerPart) / (float)segmentsPerPart;

            Vector3 firstAnchor = points[segmentStart].GetAnchorPoint();
            Vector3 secondAnchor = points[(segmentStart + 1) % points.Count].GetAnchorPoint();
            Vector3 firstControl = points[segmentStart].GetSecondControlPoint();
            Vector3 secondControl = points[(segmentStart + 1) % points.Count].GetFirstControlPoint();

            Vector3 a = Vector3.Lerp(firstAnchor, firstControl, tValue);
            Vector3 b = Vector3.Lerp(firstControl, secondControl, tValue);
            Vector3 c = Vector3.Lerp(secondControl, secondAnchor, tValue);
            Vector3 d = Vector3.Lerp(a, b, tValue);
            Vector3 e = Vector3.Lerp(b, c, tValue);
            Vector3 bez = Vector3.Lerp(d, e, tValue);

            Gizmos.DrawSphere(bez, HandleUtility.GetHandleSize(bez) * 0.01f);
            Vector3 forwardVector = (e - d).normalized;
            movable.transform.position = bez;
            movable.transform.forward = forwardVector;
        }
    }

}



}
