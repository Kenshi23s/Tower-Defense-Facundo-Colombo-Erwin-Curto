using System.Collections.Generic;
using UnityEngine;

public class AntiAirTurret : StandarTurrets
{

    [Header("MisileParameters")]
    [SerializeField] bool  infiniteDetectRadius=true;
    [SerializeField] float _misileCountDown=5;
    [SerializeField] float _explosionRadius=3;
    [SerializeField] float _minRadius=1;
    [SerializeField] float _damage = 1;


    public override void Initialize()
    {
        base.Initialize();
        shootReady = true;
        if (infiniteDetectRadius)
        {
            detectRadius = Mathf.Infinity;
        }    
    }

    public override void Shoot()
    {
        Misile _Misile = ProjectileManager.instance.GetProjectile(ProjectileType.Misile).GetComponent<Misile>();

        if (_Misile != null)
        {
            _Misile.SetParameters(_misileCountDown, _explosionRadius, _minRadius, _damage, _shootPos);
            _Misile.OnShoot();
            StartCooldown();
            //Debug.Log("Dispare");
        }
    }

    protected override bool ShootConditions()
    {

        //esto es muy overkill para chequear solo 2 booleanos pero queria armarlo para razonar como haria si tuviera
        //por ejemplo 10 condiciones 
        List<bool> conditions = new List<bool>
        {
            shootReady,
            SomeoneInSigth()
        };

        foreach (bool item in conditions)
        {
            if (item != true)
            {
                Debug.Log("condiciones false");
                return false;
            }
        }
        Debug.Log("condiciones True");
        return true;

        
    }

    protected override void StunOFF()
    {
        return;
    }

    protected override void StunOn()
    {
        return;
    }
}
