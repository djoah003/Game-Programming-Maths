using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPlacementRay : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    private Vector3 RayStart => gameObject.transform.position;
    private Vector3 Ray => transform.forward;
    private void OnDrawGizmos() => DrawRay();

    void DrawRay()
    {
        Handles.DrawLine(RayStart, RayStart + Ray * 5, 2f);
        if(Physics.Raycast(RayStart, Ray, out RaycastHit hit))
        {
            Handles.color = Color.magenta;
            Handles.DrawLine(RayStart, hit.point, 3f);

            Handles.color = Color.green;
            Handles.DrawLine(hit.point, hit.point + hit.normal, 3f);

            Handles.color = Color.red;
            Vector3 right = Vector3.Cross(hit.normal, Ray);
            Handles.DrawLine(hit.point, hit.point + 2f * right, 3f);

            Handles.color = Color.blue;
            Vector3 forward = Vector3.Cross(hit.normal, right);
            Handles.DrawLine(hit.point, hit.point + 2f * forward, 3f);
            
            Quaternion rot = Quaternion.LookRotation(forward, hit.normal);
            gameObj.transform.position = hit.point;
            gameObj.transform.rotation = rot;
        }
    }
}
