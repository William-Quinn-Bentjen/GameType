using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaPlayer : Teams.TeamMember {
    public bool LeaveTeamOnDeath = true;
    public bool Invulnerable = false;
    public bool Invincible = false;
    public float health;
    public float maxHealth;
    [Range(0,999999)]
    public float damageCurveEnd;
    public delegate void OnDeathInform(ExampleGameTypeIntegration.DeathInfo deathInfo);
    public OnDeathInform OnDeath;
    public delegate void OnSpawnInform();
    public OnSpawnInform OnSpawn;
    public AnimationCurve damageCurve;
    public MeshRenderer meshRenderer;
    public Vector3 spawnOffset;
    public Color personalColor;
    public enum InputType
    {
        keyboard,
        controller1,
        controller2,
        controller3,
        controller4,
        none
    }
    public InputType input;
    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            meshRenderer.material.color = personalColor;
        }
    }
    public void SetColor(Color color)
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null) meshRenderer.material.color = color;
    }
    private void OnCollisionEnter(Collision collision)
    {
        float force = collision.impulse.magnitude;
        TakeDamage(damageCurve.Evaluate(force / damageCurveEnd) * maxHealth, collision);
    }
    public void TakeDamage(float damage, Collision collision = null)
    {
        if (!Invulnerable && health > 0)
        {
            health -= Mathf.Abs(damage);
            if (health <= 0)
            {
                if (Invincible)
                {
                    health = .1f;
                }
                else
                {
                    Death(collision);
                }
            }
        }
    }
    public void Death(Collision collision = null)
    {
        if (OnDeath != null)
        {
            ExampleGameTypeIntegration.DeathInfo deathInfo = new ExampleGameTypeIntegration.DeathInfo();
            deathInfo.Victim = this;
            if (collision != null)
            {
                deathInfo = new ExampleGameTypeIntegration.DeathInfo(this, collision);
            }
            OnDeath.Invoke(deathInfo);
        }
    }
}
