using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : MonoBehaviour, Teams.ITeam {
    public Teams.TeamMember killer;
    public Rigidbody rb;
    public float force;
    public Teams.Team GetTeam()
    {
        if (killer == null) return null;
        return killer.team;
    }
	// Use this for initialization
	void Awake () {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                return;
            }
        }
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
