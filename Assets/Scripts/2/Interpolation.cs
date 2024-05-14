using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// LINKKI: https://youtu.be/7qKw5eDXEU0
public class Interpolation : MonoBehaviour
{
    private EasingFunction.Function easingFunction, easingFunctionRot;
    public EasingFunction.Ease easingType, easingTypeRotation;
    private GameObject GunBarrel;
    bool firstAnim = false, animFinish;

    public float Interp_time = 5.0f;

    public float elapasedTime;
    public float t;

    public float start;
    public float end;
    
    // Start is called before the first frame update
    void Start()
    {
        GunBarrel = GameObject.Find("Barrel");
        easingFunction = EasingFunction.GetEasingFunction(easingType);
        easingFunctionRot = EasingFunction.GetEasingFunction(easingTypeRotation);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = GunBarrel.transform.position;
        elapasedTime += Time.deltaTime;
        t = elapasedTime / Interp_time;

        if (animFinish) return;
        
        if (!firstAnim)
        {
            GunBarrel.transform.position = new Vector3(easingFunction(start, end, t),
                pos.y, pos.z);
                transform.rotation = Quaternion.Euler(0, 0,easingFunctionRot(0, -20, t));
            if (t > 1.0f)
            {
                firstAnim = true;
                elapasedTime = 0;
            }
        }
        else
        {
            GunBarrel.transform.position = new Vector3(easingFunction(end, start, t),
                pos.y, pos.z);
            transform.rotation = Quaternion.Euler(0, 0,easingFunctionRot(-20, 0, t));
            if (t > 1.0f)
                animFinish = true;
        }






    }
}
