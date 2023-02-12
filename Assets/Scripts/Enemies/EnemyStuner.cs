using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuner : Enemies
{
    [Header("Stuner Variables")]
    [SerializeField] float stunRadius;
    [SerializeField] float stunTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StunTurrets());
    }

    IEnumerator StunTurrets()
    {
        while (true)
        {
            if (!ScreenManager.isPaused)
            {
                yield return new WaitForEndOfFrame();
                Collider[] rangeColliders = Physics.OverlapSphere(transform.position, stunRadius);
                foreach (Collider item in rangeColliders)
                {
                    yield return new WaitForEndOfFrame();
                    Turrets turret = item.gameObject.GetComponent<Turrets>();
                    if (turret != null)
                    {
                        turret.StunTurret(stunTime);
                    }

                }
            }
            else
            {
                yield return new WaitWhile(StillPause()) ;
            }
            

        }
    }
    Func<bool> StillPause()
    {
        Func<bool> paramBuild = () => { return ScreenManager.isPaused; };

        return paramBuild;
    }
    protected override void Die()
    {
        StopCoroutine(StunTurrets());
        base.Die();
        
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
        Collider[] rangeColliders = Physics.OverlapSphere(transform.position, stunRadius);
        Gizmos.color = Color.blue;
        foreach (Collider item in rangeColliders)
        {
            Turrets turret = item.gameObject.GetComponent<Turrets>();

            if (turret != null)
            {
                
                Gizmos.DrawLine(transform.position, turret.transform.position);
            }
        }
    }

}
