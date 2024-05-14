using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vectors : MonoBehaviour
{
    public GameObject my_object;

    public void DrawVector(Vector3 pos, Vector3 v, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos + v);
        
        // Arrow head
        Handles.color = c;
        // Compute the "rough" endpoint for the cone
        // Normalize the vector (its magnitude becomes 1)
        Vector3 n = v.normalized;
        n = n * 0.35f; // Now the length is 35 cm
        
        Handles.ConeHandleCap(0, pos + v - n, Quaternion.LookRotation(v), 0.5f, EventType.Repaint);
        
    }
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(new Vector3(0, 0), new Vector3(5, 0));
        DrawVector(pos, new Vector3(5, 0), Color.red);
        DrawVector(pos, new Vector3(0, 5), Color.green);
       
        DrawVector(pos, my_object.transform.position - pos, Color.magenta);;
    }
}