using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : Teams.Base.BaseTeamObject {
    public Teams.Base.BaseTeamMember killer;
    public virtual void AssignKiller(Teams.Base.BaseTeamMember assignedKiller)
    {
        team = assignedKiller.team;
    }
	// Use this for initialization
	void Awake () {
        AssignKiller(killer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
