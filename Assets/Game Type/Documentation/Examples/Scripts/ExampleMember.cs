﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ExampleMember : Teams.TeamMember {
//    public Color personalColor = Color.clear;
//    public float deathAtImpact = 10;
//    public bool alive = true;
//    public MeshRenderer meshRenderer;
//    public MeshFilter meshFilter;
//    public Rigidbody rb;
//    public delegate void MemberOtherDelegate(Teams.TeamMember member, Teams.TeamMember other);
//    public MemberOtherDelegate OnDeath;
//    private void Reset()
//    {
//        meshFilter = GetComponent<MeshFilter>();
//        meshRenderer = GetComponent<MeshRenderer>();
//        rb = GetComponent<Rigidbody>();
//    }
//    // Use this for initialization
//    protected override void Awake()
//    {
//        meshRenderer = GetComponent<MeshRenderer>();
//        if (meshRenderer != null && personalColor != Color.clear)
//        {
//            meshRenderer.material.color = personalColor;
//        }
//        rb = GetComponent<Rigidbody>();
//        if (GameManager.Instance.GameType is ExampleGameTypeIntegration)
//        {
//            OnDeath = null;
//            OnDeath += (GameManager.Instance.GameType as ExampleGameTypeIntegration).EvaluateDeath;
//        }
//        base.Awake();
//    }
//    public void Death(Teams.TeamMember killer = null)
//    {
//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;
//        OnDeath?.Invoke(this, killer);
//    }
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (alive && collision.impulse.magnitude >= deathAtImpact)
//        {
//            ExampleBullet exampleBullet = collision.gameObject.GetComponent<ExampleBullet>();
//            if (exampleBullet != null)
//            {
//                Teams.TeamMember killer = exampleBullet.killer;
//                if (killer != null)
//                {
//                    Death(killer);
//                }
//            }
//            else
//            {
//                Death();
//            }
//        }
//    }
//    protected override bool Join(Teams.Team teamToJoin = null)
//    {
//        Teams.Team teamToBeJoined = teamToJoin ?? team;
//        if (teamToBeJoined != null)
//        {
//            if (GameManager.Instance.GameType != null)
//            {
//                if (GameManager.Instance.GameType.AttemptJoin(teamToBeJoined, this))
//                {
//                    OnJoin?.Invoke(this);
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//    protected override bool Leave()
//    {
//        if (team != null)
//        {
//            team.Leave(this);
//            GameManager.Instance.GameType.AttemptLeave(this);
//        }
//        return (team == null);
//    }
//}
