using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ProjectileData
{
    public string name;
    public Projectile model; 
    public ProjectileType type;
    
}

public class BulletPool
{
   public BulletPool() { }

    Dictionary<ProjectileType, PoolObject<Projectile>> _pools = new Dictionary<ProjectileType, PoolObject<Projectile>>();  

    public void CreateProjectileTypePool(ProjectileData data, int prewarm = 5)
    {
        if (!_pools.ContainsKey(data.type))
        {
            Func<Projectile> myBuild = ProjectileBuild(data.model);

            _pools.Add(data.type, new PoolObject<Projectile>());

            _pools[data.type].Intialize(TurnOn, TurnOff, myBuild);
            return;
        }

        Debug.Log("ya existia esta bala, no creo nada");


    }

    public Projectile AskForProjectile(ProjectileType key)
    {
        if (_pools.ContainsKey(key))
        {
            Projectile projectile = _pools[key].Get();
            projectile.Initialize(ReturnToPool, key);
            return projectile;
        }
        return null;
    }

    void TurnOn(Projectile e)
    {
        e.gameObject.SetActive(true);
    }

    void TurnOff(Projectile e)
    {
        e.gameObject.SetActive(false);
    }

    void ReturnToPool(Projectile value, ProjectileType key) => _pools[key]?.Return(value);

    Func<Projectile> ProjectileBuild(Projectile model)
    {
        
        Func<Projectile> paramBuild = () =>
        {
            Projectile projectile = GameObject.Instantiate(model);
            return projectile;
        };
        
        return paramBuild;

    }

}
