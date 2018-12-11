using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class ObjectAccelerator : MonoBehaviour {
    public float force = 30;
    public ForceMode forceMode = ForceMode.Force;
    private void OnTriggerEnter(Collider other)
    {
        Accelerate(other.gameObject.GetComponent<Rigidbody>());
    }
    private void OnTriggerStay(Collider other)
    {
        Accelerate(other.gameObject.GetComponent<Rigidbody>());
    }
    private void Accelerate(Rigidbody rb)
    {
        if (rb != null)
        {
            rb.AddForce(transform.forward * force, forceMode);
        }
    }
}
