using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaveManager
{
    public WaveManager() { }

    EnemyData[] Enemydata;
    
   [SerializeField] List<EnemyData> actualWaveData =new List<EnemyData>();

    EnemyPool pool;


    #region FlyWeigthVariables

    public static int   static_EnemiesPerWave = 4;
    public static int   static_AddperWave = 4;
    public static float static_timeBetweenWaves = 7f;
    public static float static_TimeBetweenEnemies = 0.5f;
  

    #endregion

    [SerializeField] int _enemiesPerWave = 4;
    [SerializeField] float _timeBetweenWaves = 7f;
    [SerializeField] float _timeBetweenEnemies = 0.5f;

    int _quantityToSpawn;
    [SerializeField]bool testing;
    [SerializeField]internal int actualWaveCount=0;

    


    [SerializeField] List<Enemies> _activeEnemies = new List<Enemies>();
    public List<Enemies> activeEnemiesRead => _activeEnemies;


    public void Initialize(EnemyData[] data,EnemyPool pool)
    {
        this.Enemydata = data;
        this.pool = pool;
        if (testing!=true)
        {
            _timeBetweenEnemies = static_TimeBetweenEnemies;
            _timeBetweenWaves = static_timeBetweenWaves;
            _enemiesPerWave = static_EnemiesPerWave;
        }
       

        PrepareNextWave();

    }

    public void PrepareNextWave()
    {
        actualWaveCount++;
        _enemiesPerWave += static_AddperWave;


        int quantityLeft = _quantityToSpawn = _enemiesPerWave;


        Debug.Log("tengo q spawnear "+ quantityLeft);
    
        while (quantityLeft > 0)
        {
            EnemyData data = Enemydata[UnityEngine.Random.Range(0, Enemydata.Length)];
            if (!data.canSpawn)
            {
                continue;
            }

            int actualEnemyQuantity = UnityEngine.Random.Range(1, quantityLeft);
           
            quantityLeft -= (data.Power * actualEnemyQuantity);
          
        

            while (actualEnemyQuantity > 0)
            {
                actualWaveData.Add(data);
                actualEnemyQuantity--;
            }
          
        }
       
        EnemyManager.instance.updateRefresh += TimeBetweenEnemies;
    }

    void SpawnEnemy(string type) => pool.CallEnemy(type);

    string GetEnemyWaveKey()
    {
        if (actualWaveData.Count>1)
        {
            int index = UnityEngine.Random.Range(0, actualWaveData.Count);
            string key = actualWaveData[index].type;
            actualWaveData.RemoveAt(index);
            return key;
        }
        return "";
        
    }

    #region EnemyList

    public void RemoveFromList(Enemies enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count <=0)
            {
                EnemyManager.instance.updateRefresh += TimeBetweenWaves;
            }

        }
    }

    public void AddToList(Enemies enemy)
    {
        if (!_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Add(enemy);

        }

    }

    #endregion

    #region Timers

    void TimeBetweenEnemies()
    {
        _timeBetweenEnemies -= Time.deltaTime;
       
        if (_timeBetweenEnemies <= 0)
        {
            string key = GetEnemyWaveKey();
            SpawnEnemy(key);

            _quantityToSpawn--;

            _timeBetweenEnemies = static_TimeBetweenEnemies;

            //Debug.Log("spawneo enemigo");

            if (_quantityToSpawn <= 0)
            {
                //Debug.Log("ya spawne a todos");
                EnemyManager.instance.updateRefresh -= TimeBetweenEnemies;
            }
        }
    }

    void TimeBetweenWaves()
    {
        _timeBetweenWaves -= Time.deltaTime;

        if (_timeBetweenWaves <= 0)
        {
            _timeBetweenWaves = static_timeBetweenWaves;
            PrepareNextWave();
            EnemyManager.instance.updateRefresh -= TimeBetweenWaves;
        }

    }

    #endregion
    
    #region FlyWeigthMethods

    public static void SetEnemiesPerWave(int quantity)
    {
         quantity = Mathf.Abs(quantity);
         static_EnemiesPerWave = quantity;
    }

    public static void SetEnemiesAddedPerWave(int quantity)
    {
        quantity = Mathf.Abs(quantity);
        static_AddperWave = quantity;
    }

    public static void SetTimeBetweenEnemies(float time)
    {
        time = Mathf.Abs(time);
        static_TimeBetweenEnemies = time;
    }

    public static void SetTimeBetweenWaves(float time)
    {
        time = Mathf.Abs(time);
        static_timeBetweenWaves = time;
    }
    #endregion

    //public void SpawnEnemies(string type, int quantity = 1)
    //{

    //    //if (quantity > 1)
    //    //{
    //    //    //EnemyManager.instance.updateRefresh += TimeBetweenEnemies;
    //    //}
    //    //else
    //    //{
    //    //    SpawnEnemy(type);
    //    //}
    //}

    //EnemyData GetData(List<EnemyData> dataList)
    //{
    //    EnemyData data = Enemydata[Random.Range(0, Enemydata.Length)];
    //    if (dataList.Contains(data))
    //    {
    //        return GetData(dataList);
    //    }
    //    return data;
    //}
}
