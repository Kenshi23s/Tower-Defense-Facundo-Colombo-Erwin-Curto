using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    public EnemyPool(){ camo = "_Camouflage"; }

    public string camo;
    public string camoKey => camo;
    Dictionary<string, PoolObject<Enemies>> pools = new Dictionary<string, PoolObject<Enemies>>();

    public void CreateEnemyTypePool(EnemyData data, int prewarm = 5)
    {
        string key = data.type;

        //if (data.isCamouflaged)
        //{
            
        //    key += camoKey;
        //    data.type += camoKey;
        //    //data.model.GetComponent<Renderer>().material.color = Color.red;
        //}
           

        
        if (!pools.ContainsKey(key))
        {                      
            Func<Enemies> myBuild = EnemyBuild(data);

            pools.Add(key, new PoolObject<Enemies>());
          
            pools[data.type].Intialize(TurnOn, TurnOff, myBuild);

            //data.SetPoolKey(key);
        }


    }

    Func<Enemies> EnemyBuild(EnemyData data)
    {
        Func<Enemies> paramBuild;

        if (!data.isCamouflaged)
        {
            Debug.Log(" el enemigo no contiene camo key");
            paramBuild = () =>
            {
                Enemies enemy = GameObject.Instantiate(data.model);
                return enemy;
            };
        }
        else
        {
            Debug.Log(" el enemigo CONTIENE camo key");
            paramBuild = () =>
            {
                Enemies enemy = GameObject.Instantiate(data.model);
                enemy.gameObject.layer = 3;
                enemy.GetComponent<Renderer>().material.color = Color.red;
                return enemy;
            };
        }
        //esto es magia negra
      

        return paramBuild;

    }

    public void CallEnemy(string key)
    {
        if (pools.ContainsKey(key))
        {
            Enemies enemy = pools[key].Get();
            enemy.Initialize(ReturnToPool, key);

        }
    }

    void TurnOn(Enemies e)
    {
        e.gameObject.SetActive(true);
    }

    void TurnOff(Enemies e)
    {
        e.gameObject.SetActive(false);
    }

    void ReturnToPool(Enemies value, string key) => pools[key]?.Return(value);

}
