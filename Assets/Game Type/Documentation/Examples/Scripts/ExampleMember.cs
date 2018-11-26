using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleMember : Teams.TeamMember {
    public float deathAtImpact = 10;
    public bool alive = true;
    public MeshRenderer meshRenderer;
    public Rigidbody rb;
    public delegate void MemberOtherDelegate(Teams.TeamMember member, Teams.TeamMember other);
    public MemberOtherDelegate OnDeath;
    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    protected override void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        base.Awake();
    }
    public void Death(Teams.TeamMember killer = null)
    {
        OnDeath(this, killer);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (alive && collision.impulse.magnitude >= deathAtImpact)
        {
            ExampleBullet exampleBullet = collision.gameObject.GetComponent<ExampleBullet>();
            if (exampleBullet != null)
            {
                Teams.TeamMember killer = exampleBullet.killer;
                if (killer != null)
                {
                    Death(killer);
                }
            }
            else
            {
                Death();
            }
        }
    }
    protected override bool Join(Teams.Team teamToJoin = null)
    {
        Teams.Team teamToBeJoined = teamToJoin ?? team;
        if (teamToBeJoined != null)
        {
            if (GameManager.Instance.GameType != null)
            {
                if (GameManager.Instance.GameType.AttemptJoin(teamToBeJoined, this))
                {
                    if (OnJoin != null) OnJoin(this);
                    return true;
                }
            }
        }
        return false;
    }
    protected override bool Leave()
    {
        if (team != null)
        {
            team.Leave(this);
            GameManager.Instance.GameType.AttemptLeave(this);
        }
        return (team == null);
    }
}
