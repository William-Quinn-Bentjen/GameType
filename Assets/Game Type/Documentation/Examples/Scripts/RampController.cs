using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampController : MonoBehaviour {

    public const float maxYaw = 15;
    public const float maxForce = 1000;


    public ObjectAccelerator objectAccelerator;
    [Range(1,89)]
    public float pitch = 0;
    [Range(-maxYaw,maxYaw)]
    public float yaw = 0;
    [Range(0, maxForce)]
    public float force = 200;
    public static float rotationFaultTolerance = 0.01f;
    public float Pitch 
    {
        get
        {
            return transform.rotation.eulerAngles.x;
        }
        set
        {
            if (Mathf.Abs(value - transform.rotation.eulerAngles.x) >= rotationFaultTolerance && value <= 90 && value >= 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(value, transform.localEulerAngles.y, transform.localEulerAngles.z));
            }
        }
    }
    public float Yaw
    {
        get
        {
            return transform.rotation.eulerAngles.y;
        }
        set
        {
            if (Mathf.Abs(value - transform.rotation.eulerAngles.y) >= rotationFaultTolerance && value <= maxYaw && value >= -maxYaw)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.localEulerAngles.x, value, transform.localEulerAngles.z));
            }
        }
    }
    public float Force
    {
        get
        {
            return objectAccelerator.force;
        }
        set
        {
            
            objectAccelerator.force = Mathf.Clamp(value, 0, maxForce);
        }
    }
    private void Reset()
    {
        objectAccelerator = GetComponentInChildren<ObjectAccelerator>();
    }

    // Update is called once per frame
    void Update () {
        RotateHinge();
        SetForce();
	}
    void RotateHinge()
    {
        Pitch = pitch;
        Yaw = yaw;
    }
    void SetForce()
    {
        Force = force;
    }
}
