using FacundoColomboMethods;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct EnemyData
{
    public string type;

    //private string poolKey;
    //public void SetPoolKey(string key)
    //{

    //    Debug.Log(poolKey+0);
    //    poolKey=key;
    //    Debug.Log(poolKey+1);

    //}

    //public string GetPoolKey()
    //{
    //    Debug.Log(poolKey);
    //    return poolKey;

    //}

    public bool isCamouflaged;

    public Enemies model;


    public bool canSpawn;

    [SerializeField]private int power;

    public int Power { get => Mathf.Clamp(power,1,10); set => power = value; }
   
}


public class EnemyManager : MonoSingleton<EnemyManager>, IPausable
{
    public int wavesSurvived => waveManager.actualWaveCount;
    public LayerMask camouflageMask;

    public EnemyData[] enemyDatas;

    EnemyPool pool = new EnemyPool();

    [SerializeField] WaveManager waveManager;

    public List<Enemies> activeEnemies { get => waveManager.activeEnemiesRead; }

    #region Waypoint

    [SerializeField] Transform waypointFather;
    
    List<Vector3> _waypoints = new List<Vector3>();
    public List<Vector3> waypoints { get => _waypoints; }

    #endregion

    public event Action updateRefresh;

    bool isPaused;

    public void Initialize()
    {
        ArtificialAwake();
        ScreenManager.AddPausable(this);

        Transform[] aux = ColomboMethods.GetChildrenComponents<Transform>(waypointFather);
        _waypoints = new List<Vector3>();

        for (int i = 0; i < aux.Length; i++)
        {
            _waypoints.Add(aux[i].position);
            aux[i].name = "Waypoint " + i.ToString();
        }
        InitializePool();
        waveManager.Initialize(enemyDatas, pool);
    }

    void InitializePool() => enemyDatas.ToList().ForEach(x => pool.CreateEnemyTypePool(x));

    private void LateUpdate()
    {
        if (!isPaused)
            updateRefresh?.Invoke();

    } 

    #region ListMethods
public void AddToList(Enemies enemy)=> waveManager.AddToList(enemy);


    public void RemoveFromList(Enemies enemy)=> waveManager.RemoveFromList(enemy);

    #endregion

    #region PauseManager
    public void Pause() => isPaused = true;

    public void Resume() => isPaused = false;
  
    #endregion

    private void OnDrawGizmos()
    {
        if (waypoints.Count > 0)
        {
            Gizmos.color = Color.blue;

            //puntos
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.DrawWireSphere(waypoints[i], 0.5f);
            }

            Gizmos.color = Color.black;

            //uniones
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }
        }
    }

    //public void SpawnEnemy(string type) => pool.CallEnemy(type);

    //public void SpawnEnemies(string type, int quantity)
    //{
    //    if (quantity>1)
    //    {
    //        StartCoroutine(SpawnGroup(type, quantity));
    //    }
    //    else
    //    {
    //        SpawnEnemy(type);
    //    }
    //}

    //IEnumerator SpawnGroup(string type,int quantity)
    //{

    //    for (int i = 0; i < quantity; i++)
    //    {
    //        if (isPaused)
    //        {
    //            yield return new WaitWhile(StillPause());
    //        }

    //        SpawnEnemy(type);

    //        yield return new WaitForSeconds(0.5f);
    //    }

    //}

    //Func<bool> StillPause()
    //{
    //   Func<bool> paramBuild = () => { return isPaused; };

    //   return paramBuild;
    //}
    //void InitializePool()
    //{
    //    for (int i = 0; i < enemyDatas.Length; i++)
    //        pool.CreateEnemyTypePool(enemyDatas[i]);
    //}
}
