using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleInfectedMember : ExampleMember {
    public float attackDistance = 1;
    private void Awake()
    {
        personalColor = Color.clear;
        base.Awake();
    }
    private void FixedUpdate()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, attackDistance))
        {
            ExampleMember memberCheck = GetComponent<ExampleMember>();
            if (memberCheck != null)
            {
                if (memberCheck.team != team)
                {
                    memberCheck.OnDeath(memberCheck, this);
                }
            }
        }
    }
}
