using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = Camera.main.transform.position - transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.position = transform.position + offset;
	}
}
