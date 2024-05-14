using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
using UnityEngine;

public class InterpolationVectors : Vectors
{

    public GameObject goA;
    public GameObject goB;
    public float Interp_time = 5.0f;

    [Range(0f,10f)]
    public float elapasedTime;
    public float t;

    private void OnDrawGizmos()
    {
        DrawVector(Vector3.zero, goA.transform.position, Color.green);
        DrawVector(Vector3.zero, goB.transform.position, Color.red);
        //Draw the "parts" of vectors accourding to the interpolation
        DrawVectorParts(t);
    }
    void Start()
    {
        elapasedTime = 0.0f;
    }

    void DrawVectorParts(float t)
    {
        Vector3 partOfA = (1 - t) * goA.transform.position;
        Vector3 partOfB = t * goB.transform.position;

        //Draw the vector parts
        DrawVector(Vector3.zero, partOfA, Color.magenta);
        DrawVector(partOfA, partOfB, Color.magenta);

        //DrawVector();
    }
    void Update()
    {
        //Lets get the elapsed time
        elapasedTime += Time.deltaTime;

        // Interpolate until Interp_time
        t = elapasedTime / Interp_time;
        
        //Clamp the t to the 1 (remember t has to be between 0 and 1)
        if (t > 1.0f)
            t= 1.0f;
        // Easing?
        if (t < 0.5F)
        {
            t = 2 * t * t; // y = 2 * x^2
        }
        else
        {
            t = 1 - 2 * (1 - t) * (1 - t); // y = 1 - 2 * (1-x)^2
        }

        //Compute the interpolation f(t) = A*(1-t) + B*t
        Vector3 pos = (1-t) * goA.transform.position + t * goB.transform.position;
        //Set the my_object position
        my_object.transform.position = pos;

        
    }

}