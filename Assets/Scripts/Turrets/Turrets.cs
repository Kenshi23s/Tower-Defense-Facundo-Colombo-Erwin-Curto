using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;
using System;

public abstract class Turrets : MonoBehaviour,IPausable
{
    [SerializeField]protected LayerMask CanSee; 
    public Enemies _target;
    public Transform _shootPos;

    [SerializeField]protected int damage = 1;
    [SerializeField]protected float detectRadius = 5f;
    [SerializeField]protected float stunedFor = 5f;

    protected event Action timersUpdate;

    protected bool canShoot;
    protected bool stuned;
    #region Stun

    //queria chequear si esto lo podia hacer asi, no fue posible
    //Action _timerStun = () =>
    //{
    //    stunedFor -= Time.deltaTime;
    //    if (stunedFor<=0)
    //    {
    //        stunedFor = 0;
    //        stuned = false;
    //        timersUpdate -= _timerStun;
    //    }
    //};

   
    public virtual void StunTurret(float time)
    {
        if (Mathf.Abs(time) > stunedFor)
        {
            stunedFor = Mathf.Abs(time);
            stuned = true;
            StunOn();
            timersUpdate += TimerStun;
        }
            
    }
   


    void TimerStun()
    {
        stunedFor -= Time.deltaTime;
        if (stunedFor <= 0)
        {
            stunedFor = 0;
            stuned = false;
            timersUpdate -= TimerStun;
            StunOFF();
        }
    }
    #endregion

    #region Abstractions
    public abstract void Shoot();

    protected abstract void StunOn();

    protected abstract void StunOFF();

    protected abstract bool ShootConditions();
    #endregion

    private void Awake()
    {
        ScreenManager.AddPausable(this);
        canShoot = true;
        stunedFor = 0;
        Initialize();     
        
    }
    public virtual void Initialize() { }

    #region ScreenManager

    public virtual void Pause()
    {
       canShoot = false;
    }

    public virtual void Resume()
    {
       canShoot = true;
    }

    #endregion

    protected virtual void Update()
    {
        if (!ScreenManager.isPaused)
        {
            if (ShootConditions() && !stuned)
            {
                Shoot();
            }
            timersUpdate?.Invoke();
        }
    }

    #region EnemyCheck

    protected Enemies GetNearestEnemy()
    {
        if (EnemyManager.instance.activeEnemies.Count>=1)
        {
            Enemies enemy = ColomboMethods.GetNearestOnSigth(EnemyManager.instance.activeEnemies, _shootPos.position, GameManager.instance.wallMask);
            if (enemy != null)
            {
                Debug.Log("no es null,obtengo al enemigo mas cercano");
                return enemy;
            }
        }
        return null;       
    }

    protected bool SomeoneInSigth()
    {
        bool InSigthWall = ColomboMethods.CheckNearbyInSigth<Enemies>(_shootPos.position, detectRadius, GameManager.instance.wallMask);

        bool canSeeEnemy = !ColomboMethods.CheckNearbyInSigth<Enemies>(_shootPos.position, detectRadius, CanSee);

       
        if (canSeeEnemy && InSigthWall)
        {
            Debug.Log("veo al enemigo");
            return true;
        }

        return false;
    }
    #endregion

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_shootPos.position, detectRadius);
    }

}
