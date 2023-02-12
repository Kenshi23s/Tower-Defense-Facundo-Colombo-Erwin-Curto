using System;
using UnityEngine;

public class RayTurret : Turrets
{
    [Header("RayTurret")]
    [SerializeField] LineRenderer laser;
    [SerializeField] Transform head;
    int damageAux;
    float damageMultiplier;

    public override void Initialize()
    {
        base.Initialize();      

        SetLaserPos(0, _shootPos.position,false);
       
        damageMultiplier = 0;


    }
  
    public override void Shoot()
    {      
        Vector3 dir = _target.transform.position - transform.position;
        head.forward = dir.normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, detectRadius, CanSee)&&!stuned)
        {
            damageMultiplier += Time.deltaTime;

            hit.transform.GetComponent<Enemies>()?.TakeDamage(damage* damageMultiplier);

            SetLaserPos(1, hit.point, true);

            return;
        }
       
          damageMultiplier = 0;        
          SetLaserPos(1, _shootPos.position, false);
          _target = null;
           

      
        
    }
    void SetLaserPos(int index, Vector3 pos,bool value)
    {
        laser.enabled = value;
        laser.SetPosition(index, pos);
        if (value == true)
        {
            Debug.Log("se activo el laser");
        }
        else
        {
            Debug.Log("se desactivo el laser");
        }
    }
   

    

    protected override bool ShootConditions()
    {
        if (_target==null||!_target.stillAlive)
        {
            if (SomeoneInSigth())
            {
                _target = GetNearestEnemy();

                if (_target != null&&_target.isActiveAndEnabled)
                {
                    //Debug.Log("el enemigo no es igual a null,se puede disparar");
                    damageMultiplier = 0;
                    return true;
                }

            }
            SetLaserPos(1, _shootPos.position, false);
            return false;

        }
        return true;
       
    }

  
    protected override void StunOn()
    {
        SetLaserPos(1, _shootPos.position, false);
        _target = null;
    }

    protected override void StunOFF()
    {
        return;
    }

    public override void Pause()
    {
        damageAux = damage;
        damage = 0;
    }
    public override void Resume()
    {
        damage = damageAux;
    }
}
