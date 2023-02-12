using System;
using System.Collections;
using UnityEngine;

public abstract class StandarTurrets : Turrets
{
    [Header("StandarTurret")]
    public float cooldown;
    [SerializeField]protected bool shootReady;

    public override void Initialize()
    {
        base.Initialize();
        shootReady = true;
    }

    protected void StartCooldown() => StartCoroutine(CooldownCoroutine());

    IEnumerator CooldownCoroutine() 
    {
        shootReady = false;

        if (!canShoot)
        {
            yield return new WaitUntil(StillPause());
        }
        
        yield return new WaitForSeconds(cooldown);
        shootReady = true;
    }

    Func<bool> StillPause()
    {
        Func<bool> paramBuild = () => { return shootReady; };

        return paramBuild;
    }

}
