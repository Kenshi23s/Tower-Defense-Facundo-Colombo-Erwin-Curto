using System;
using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;

public abstract class Enemies : MonoBehaviour, IPausable
{
    [SerializeField] protected float nodeRange = 2;
    bool _stillAlive;
    public bool stillAlive => _stillAlive;
    protected List<Vector3> _ActualPath = new List<Vector3>();

    [Header("Healh")]
    [SerializeField] float life;
    public float maxLife;

    [Header("AtkStats")]
    [SerializeField]protected int DamageToBase;

    [Header("SpeedStats")]
    public float speed = 5;
    float auxspeed;
   

    protected Action<Enemies, string> returnToPool;
    string myPoolType;

    public virtual void Initialize(Action<Enemies, string> returnPool, string myType)
    {
        EnemyManager.instance.AddToList(this);
        ScreenManager.AddPausable(this);

        this.returnToPool = returnPool;
        this.myPoolType = myType;

        _stillAlive=true;
        life = maxLife;

        _ActualPath = ColomboMethods.CloneList(EnemyManager.instance.waypoints);
        transform.position = _ActualPath[0];

        if (myType.Contains("_Camouflage"))
        {
            gameObject.layer = EnemyManager.instance.camouflageMask;
        }

    } 

    protected virtual void Die()
    {
        EnemyManager.instance.RemoveFromList(this);
        ScreenManager.RemovePausable(this);
        _stillAlive=false;
        returnToPool.Invoke(this,myPoolType);
    }

    private void Update()
    {
        if (!ScreenManager.isPaused)
        {
            EnemyUpdate();
        }
      
    }

   protected virtual void EnemyUpdate()=> MovePath();

    public virtual void MovePath()
    {
        if (_ActualPath.Count >= 1)
        {
            Vector3 dir = _ActualPath[0] - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
            if (nodeRange > dir.magnitude)
            {
                _ActualPath.RemoveAt(0);
            }
        }
        else
        {
            GameManager.instance.SubstractBaseLife(DamageToBase);
            Die();
        }
          
    } 

    public void TakeDamage(float value)
    {
         life -= value;
         life = Mathf.Clamp(life, 0, maxLife);
    
         if (life==0)
         {
             Die();
         }
    }
    
    public virtual void Pause()
    {
        auxspeed = speed;
        speed = 0;
    }

    public virtual void Resume()
    {
        speed = auxspeed;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, nodeRange);
        if (_ActualPath.Count>=1)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _ActualPath[0]);

        }
        
    }



}
