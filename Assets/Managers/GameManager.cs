using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public EnemyManager enemyManager;

    public static int static_WavesSurvived = 0;

    public LayerMask wallMask;

    [SerializeField] int actualLife;
    [SerializeField] int maxLife;

    [SerializeField] Slider gamebar;

    public bool ispaused;


    public void SubstractBaseLife(int mainvalue)
    {
        actualLife -= Mathf.Abs(mainvalue);
        gamebar.value = (actualLife * 1.0f) / maxLife;

        if (actualLife <= 0)
        {
            static_WavesSurvived = enemyManager.wavesSurvived;

            GoToMenu();
        }
    }

    void GoToMenu() => SceneManager.LoadScene(2);

 

    private void Awake()
    {
        ArtificialAwake();
        actualLife = maxLife;
        ispaused = false;
        Application.targetFrameRate = 144;
        enemyManager.Initialize();

    }
  

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!ispaused)
            {
                ScreenManager.PauseGame();
                ispaused = !ispaused;
                Debug.Log("PAUSO");
            }
            else
            {
                ScreenManager.ResumeGame();
                ispaused = !ispaused;
                Debug.Log("DESPAUSO");
            }
            
        }
        
    }

}
