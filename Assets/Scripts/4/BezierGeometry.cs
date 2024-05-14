using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierGeometry : MonoBehaviour
{
    [SerializeField] public Transform anchor; // GUARANTEED TO BE ON THE PATH. STATIC.
    [SerializeField] public Transform[] controls = new Transform[2]; // RELATIVE to the path.

    // Helpers
    public Vector3 GetAnchorPoint() => anchor.position;
    public Vector3 GetFirstControlPoint() => controls[0].position;
    public Vector3 GetSecondControlPoint() => controls[1].position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
       // Gizmos.DrawLine(GetFirstControlPoint(), GetAnchorPoint());
        //Gizmos.DrawLine(GetAnchorPoint(), GetSecondControlPoint());
        
        Gizmos.color = Color.magenta;
       // Gizmos.DrawSphere(GetFirstControlPoint(), HandleUtility.GetHandleSize(GetFirstControlPoint()) * 0.20f);
        //Gizmos.DrawSphere(GetSecondControlPoint(), HandleUtility.GetHandleSize(GetSecondControlPoint()) * 0.20f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetAnchorPoint(), HandleUtility.GetHandleSize(GetAnchorPoint()) * 0.01f);
    }
}
