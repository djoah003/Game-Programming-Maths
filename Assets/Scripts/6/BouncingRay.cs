using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BouncingRay : MonoBehaviour
{
    private Vector3 RayStart => gameObject.transform.position;
    private Vector3 Ray => transform.forward;
    [Range(1f, 20f)] [SerializeField] private float distance;
    [Range(1, 100)] [SerializeField] private int n;
    private void OnDrawGizmos() => DrawRay();

    void DrawRay()
    {
        if (Physics.Raycast(RayStart, Ray, out RaycastHit hit, distance))
        {
            Handles.DrawLine(RayStart, hit.point, 2f);
            DrawReflection(Ray, hit);
        }
        else
            Handles.DrawLine(RayStart, Ray * distance, 2);
        
    }

    void DrawReflection(Vector3 rayDir, RaycastHit hit, int count = 0)
    {
        if (count >= n) return;
        Vector3 reflectionDir = (rayDir - (2 * Vector3.Dot(rayDir, hit.normal) * hit.normal)) * distance;
        Vector3 hitPos = hit.point;

        if (Physics.Raycast(hitPos, reflectionDir, out RaycastHit reflectionHit, distance))
        {
            Handles.color = Color.red;
            Handles.DrawLine(hitPos, reflectionHit.point, 2f);
            
            Handles.color = Color.green;
            Handles.DrawLine(hitPos, hitPos + hit.normal, 2);
            
            DrawReflection(reflectionDir, reflectionHit, count + 1);  
        }
    }
}
