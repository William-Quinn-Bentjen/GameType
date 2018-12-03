using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {
    public Transform target;
    public float speed = 5;
    public float stopDistance = .25f;
    // Update is called once per frame
    void FixedUpdate () {
        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            // Disable if within stop distance
            enabled = false;
        }
        // Move to target
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
    }
}
