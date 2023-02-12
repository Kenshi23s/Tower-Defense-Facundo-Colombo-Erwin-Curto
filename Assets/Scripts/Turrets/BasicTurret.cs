using UnityEngine;

public class BasicTurret : StandarTurrets
{
    [SerializeField] Transform _turretHead; 
    public override void Shoot()
    {
        //transform.forward = _shootPos.forward = dir.normalized;
        Bullet bullet =  ProjectileManager.instance.GetProjectile(ProjectileType.Bullet).GetComponent<Bullet>();

        if (bullet!=null)
        {
            bullet.SetParameters(_shootPos);
            StartCooldown();
        }
       
    }

    protected override void Update()
    {
        base.Update();
        if (_target!=null && Vector3.Distance(transform.position,_target.transform.position) < detectRadius)
        {
            Vector3 dir = _target.transform.position - _turretHead.position;
            _turretHead.right = dir.normalized;
        }
        else if(SomeoneInSigth())
        {
            _target = GetNearestEnemy();
           
        }
       

    }


    protected override bool ShootConditions()
    {
        Enemies temp = null;
        if (_target != null)
        {
            if (SomeoneInSigth() && !_target.stillAlive)
            {
                temp = GetNearestEnemy();
                _target = temp;
            }
        
        }
        else
        {
            if (SomeoneInSigth())
            {
                temp = GetNearestEnemy();
                _target = temp;
            }

        }

        if (shootReady && _target != null)
        {
            if (Vector3.Distance(transform.position, _target.transform.position) < detectRadius)
            {
                return true;
            }
          
        }

         return false;
    }
    protected override void StunOFF()
    {
        return;
    }

    protected override void StunOn()
    {
        return;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (_target!=null)
        {
            Gizmos.DrawWireSphere(_target.transform.position, 6f);
        }
        //Gizmos.DrawLine(transform.position, transform.position +dir);
    }
}
