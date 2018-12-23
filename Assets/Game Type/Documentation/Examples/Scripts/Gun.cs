using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public ExampleBullet projectile;
    public Transform muzzle;
    public float TBS = .2f;
    public float TBSTimer = 0;
    public void Fire(Teams.TeamMember killer)
    {
        if (TBSTimer > TBS && projectile != null)
        {
            Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation).Fire(killer, this);
            TBSTimer = 0;
        }
    }
    private void Start()
    {
        StartCoroutine(TBSTimerCoroutine());
    }
    IEnumerator TBSTimerCoroutine()
    {
        while (true)
        {
            TBSTimer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
