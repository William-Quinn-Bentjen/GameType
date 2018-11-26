using System.Collections;
using System.Collections.Generic;
using Teams.Base;
using UnityEngine;

public class ExampleMember : Teams.Base.BaseTeamMember {
    public float deathAtImpact = 10;
    public bool alive = true;
    public MeshRenderer meshRenderer;
    public Rigidbody rb;
    public delegate void MemberOtherDelegate(BaseTeamMember member, BaseTeamMember other);
    public MemberOtherDelegate OnDeath;
    // Use this for initialization
    protected override void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        base.Awake();
    }
    public void Death(BaseTeamMember killer = null)
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
                BaseTeamMember killer = exampleBullet.killer;
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
}
