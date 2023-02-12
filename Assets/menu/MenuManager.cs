using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelData
{
    public string nameDificult;

    public int EnemiesPerWave;
    public int AddperWave;
    public float timeBetweenWaves;
    public float TimeBetweenEnemies;
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField]
    public List<LevelData> DificultLevel = new List<LevelData>();

    public string typemenuselected = "menu";
    public Dictionary<string,Menu> menus = new Dictionary<string,Menu>();

    public void Start()
    {
        instance = this;
        ConfigAllMenus();
    }

    public void ChangeMenu(string changemenu)
    {
        typemenuselected = changemenu;

        foreach (var item in menus.Keys)
        {
            if (menus[item].MenuName == typemenuselected)
            {
                menus[item].gameObject.SetActive(true);
            } 
            else
            {
                menus[item].gameObject.SetActive(false);
            }
        }
 
    }

    public void OnQuitGame()
    {
        Application.Quit();        
    }

    public void AddMenu(string name,Menu menuadd)
    {
        if (!menus.ContainsKey(name))
        {
            menus.Add(name,menuadd);
        }        
    }

    public void ConfigAllMenus()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Menu temp = this.transform.GetChild(i).GetComponent<Menu>();

            if (temp != null)
            {
                menus.Add(temp.MenuName,temp);
            }
        }




    }

    public void ExecuteDificultLevel(LevelData mainleveldata)
    {

        WaveManager.SetEnemiesPerWave(mainleveldata.EnemiesPerWave);
        WaveManager.SetEnemiesAddedPerWave(mainleveldata.AddperWave);
        WaveManager.SetTimeBetweenEnemies(mainleveldata.TimeBetweenEnemies);
        WaveManager.SetTimeBetweenWaves(mainleveldata.timeBetweenWaves);

        SceneManager.LoadScene("GameScene");
    }

    public void ExecuteDificultLevel(int indexLeveldata)
    {
        LevelData mainleveldata = DificultLevel[indexLeveldata];

        WaveManager.SetEnemiesPerWave(mainleveldata.EnemiesPerWave);
        WaveManager.SetEnemiesAddedPerWave(mainleveldata.AddperWave);
        WaveManager.SetTimeBetweenEnemies(mainleveldata.TimeBetweenEnemies);
        WaveManager.SetTimeBetweenWaves(mainleveldata.timeBetweenWaves);

        SceneManager.LoadScene("GameScene");
    }
}
