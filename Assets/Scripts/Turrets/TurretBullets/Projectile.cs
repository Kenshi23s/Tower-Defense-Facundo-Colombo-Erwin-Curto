using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour,IPausable
{
    protected Action<Projectile, ProjectileType> returnToPool;
    protected Rigidbody rb;

    [Header("ProjectileStats")]
    [SerializeField] protected float damage;
    [SerializeField]protected float speed;
    float auxSpeed;

    ProjectileType key;

    public abstract void OnShoot();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ScreenManager.AddPausable(this);
    }
    public virtual void Initialize(Action<Projectile, ProjectileType> returnToPool,ProjectileType key)
    {
        this.returnToPool = returnToPool;
        this.key = key;
    }

    public virtual void Pause()
    {
        auxSpeed = speed;
        speed = 0;
    }

    public virtual void Resume()
    {
        speed = auxSpeed;
    }

    protected virtual void Destroy()
    {
        returnToPool.Invoke(this,key);
      
    }

 
}
