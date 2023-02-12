using FacundoColomboMethods;
using System;
using UnityEngine;

public class Misile : ProjectileUpdate
{
    [Header("MisileStats")]
    float _countBeforeLock;
    float _explosionRadius;

    [SerializeField]Enemies _target;
    Action _actualState;

    public override void Initialize(Action<Projectile, ProjectileType> returnToPool, ProjectileType key)
    {
        base.Initialize(returnToPool, key);
    }

    public void SetParameters(float countBeforeLock, float explosionRadius, float minDistance,float _damage,Transform canon)
    {
        this._countBeforeLock = countBeforeLock;
        this._explosionRadius = explosionRadius;
        transform.position = canon.transform.position;
        transform.forward = canon.transform.forward;
        this.damage = _damage;
    }
   
    public override void EveryTick() => _actualState?.Invoke();

    public override void OnShoot() => _actualState = Countdown;
 
    void Countdown()
    {

        _countBeforeLock = Mathf.Clamp(_countBeforeLock,0, _countBeforeLock-Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed * ScreenManager.time;

        if (_countBeforeLock<=0)
        {
            _target = ColomboMethods.GetNearestOnSigth(EnemyManager.instance.activeEnemies, transform.position, GameManager.instance.wallMask);
            if (_target!=null)
            {
                _actualState = PursuitTarget;
                return;
            }
            Destroy();

        }
    }

    void PursuitTarget()
    {
        if (_target.stillAlive&& _target.isActiveAndEnabled)
        {
            Vector3 dir = _target.transform.position - transform.position;
                  
            transform.position += dir * Time.deltaTime * speed * (dir.magnitude/5);
            transform.forward = dir.normalized;
            if (dir.magnitude < _explosionRadius)
            {
                Destroy();
            }
        }
        else
        {
            _actualState = Countdown;
        }
       

       
    }

    protected override void Destroy()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider item in colliders)
        {
            Enemies enemy = item.GetComponent<Enemies>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        base.Destroy();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (true)
        {
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
       
    }






}
