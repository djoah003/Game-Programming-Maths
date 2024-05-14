using System;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


/// VIDEO https://youtu.be/0EuyaRBUNRY
public class LookAtTrigger : MonoBehaviour
{
    [SerializeField] private float circRadius;
    //[SerializeField] private Vector3 _circRadiusPos;
    [SerializeField] GameObject playerPos;
    // [SerializeField] GameObject lookAtPosition;
    // [Range(-180f, 180f)] 
    // [SerializeField] private float _angleThreshold;

    public float circHeight;
    [Range(-1f, 1f)] 
    private float dotThreshold;
    Vector3 Position => transform.position;
    private Vector3 VectorPos => playerPos.transform.position;
    public Light light;

    private void OnDrawGizmos()
    {
;
        // Vector3 lookAtPos = lookAtPosition.transform.position - position;

        // Vector3 l = lookAtPos.normalized;
        // Vector3 n = (vectorPos - position).normalized;
        
        // LOOK AT vector
        // Drawing.DrawVector(position, lookAtPos, Color.magenta, 1.5f);
        //Drawing.DrawVector(position, l * circRadius, Color.white, 1.5f);
        
        // // PLAYER vector
        // float dot = Vector3.Dot(l, n);
        // float DotProdThreshold = Mathf.Cos(Mathf.Deg2Rad * _angleThreshold);
        // Drawing.DrawVector(position, vectorPos - position, dot > DotProdThreshold
        //     ? Color.green // now you see me
        //     : Color.blue, // now you don't
        //     1.5f);

        // // CIRCLE
        // Handles.color = Vector3.Distance(Position, VectorPos) >= circRadius
        //     ? Color.green
        //     : Color.red;
        // Handles.DrawWireDisc(Position, transform.forward, circRadius, 1.5f);
        //
        // // Wedge
        // Handles.color = Color.white;
        // // lower disc
        // Handles.DrawWireDisc(Position - circHeight / 2.0f * transform.forward, transform.forward, circRadius, 2f);
        // // upper disc 
        // Handles.DrawWireDisc(Position + circHeight / 2.0f * transform.forward, transform.forward, circRadius, 2f);

        // Quaternion for rotation
        
        // // Upper
        // Quaternion q_rot = Quaternion.Euler(0, 0, _angleThreshold);
        // Vector3 rotated = q_rot * l;
        // // Drawing.DrawVector(position, rotated * circRadius, Color.yellow, 2f);
        //
        // // Lower
        // q_rot = Quaternion.Euler(0, 0, -_angleThreshold);
        // Vector3 rotated2 = q_rot * l;
        // // Drawing.DrawVector(position, rotated2 * circRadius, Color.yellow, 2f);
        //
        // // Upper disk lines
        // Gizmos.DrawLine(position + transform.forward * circHeight / 2f, position + transform.forward * circHeight / 2f + rotated * circRadius);
        // Gizmos.DrawLine(position - transform.forward * circHeight / 2f, position - transform.forward * circHeight / 2f + rotated * circRadius);
        // Gizmos.DrawLine(position + transform.forward * circHeight / 2f + rotated * circRadius, position - transform.forward * circHeight / 2f + rotated * circRadius);
        //
        // // Middle
        // Gizmos.DrawLine(position - transform.forward * circHeight / 2f, position + transform.forward * circHeight / 2f);
        //
        // // Lower disk lines
        // Gizmos.DrawLine(position + transform.forward * circHeight / 2f, position + transform.forward * circHeight / 2f + rotated2 * circRadius);
        // Gizmos.DrawLine(position - transform.forward * circHeight / 2f, position - transform.forward * circHeight / 2f + rotated2 * circRadius);
        // Gizmos.DrawLine(position + transform.forward * circHeight / 2f + rotated2 * circRadius, position - transform.forward * circHeight / 2f + rotated2 * circRadius);

    }

    private void Update()
    {
        light.intensity = Vector3.Distance(Position, VectorPos) >= circRadius
            ? light.intensity = 0
            : light.intensity = 15;
    }
}