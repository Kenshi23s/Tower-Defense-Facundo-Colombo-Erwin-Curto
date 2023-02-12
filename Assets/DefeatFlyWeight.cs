using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatFlyWeight : MonoBehaviour
{
    [SerializeField]Text defeatRounds;
    private void Start()
    {
        string rounds= GameManager.static_WavesSurvived.ToString();
        defeatRounds.text = "Perdiste,sobreviviste " + rounds + " rondas";
    } 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene(0);
        }
    }
}
