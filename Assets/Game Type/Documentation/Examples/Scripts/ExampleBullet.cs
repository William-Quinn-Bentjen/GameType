using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Teams.Base.BaseTeamObject {
    public Teams.Base.BaseTeamMember killer;
    public Rigidbody rb;
    public float force;
    public virtual void AssignKiller(Teams.Base.BaseTeamMember assignedKiller)
    {
        team = assignedKiller.team;
    }
	// Use this for initialization
	void Awake () {
        AssignKiller(killer);
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
