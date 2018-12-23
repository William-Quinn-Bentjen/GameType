using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : MonoBehaviour, Teams.ITeam {
    public GameObject Gun;
    public Teams.TeamMember Killer;
    public Rigidbody rb;
    public float force;
    public Teams.Team GetTeam()
    {
        if (Killer == null) return null;
        return Killer.team;
    }
	// Use this for initialization
	void Awake () {
	}
    public void Fire(Teams.TeamMember killer = null, Gun gun = null)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                return;
            }
        }
        if (killer != null && Killer != killer)
        {
            Killer = killer;
        }
        if (gun != null) Gun = gun.gameObject;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
