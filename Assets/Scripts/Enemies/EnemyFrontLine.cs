using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrontLine : Enemies
{
    Vector3 end;
    public override void Initialize(Action<Enemies, string> returnPool, string myType)
    {
        base.Initialize(returnPool, myType);
        end = _ActualPath[_ActualPath.Count - 1];
    }

    public override void MovePath()
    {
        Vector3 dir = end - transform.position;
        if (dir.magnitude > nodeRange)
        {
            transform.position += dir.normalized * Time.deltaTime * speed;
            transform.forward= dir;          
        }
        else
        {
            GameManager.instance.SubstractBaseLife(DamageToBase);
            Die();
        }  
    }
    private void OnTriggerEnter(Collider other)
    {
        var turret=other.gameObject.GetComponent<Turrets>();
        Debug.Log(turret);
        if (turret != null)
        {
            Destroy(turret.gameObject);
            Die();
        }
    }
}
