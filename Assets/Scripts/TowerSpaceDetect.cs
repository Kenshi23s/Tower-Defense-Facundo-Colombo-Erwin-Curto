using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpaceDetect : MonoBehaviour
{   
    [System.NonSerialized]
    public int spaceBlocked;

    public Material blockedMat;

    public Material freeMat;

    public GameObject Body, Head, Turret;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        spaceBlocked++;
        ChangeMaterial();
    }

    private void OnTriggerExit(Collider other)
    {
        spaceBlocked--;
        ChangeMaterial();
    }

    public void ChangeMaterial()
    {
        if (spaceBlocked > 0)
        {
            Body.GetComponent<Renderer>().material = blockedMat;
            Head.GetComponent<Renderer>().material = blockedMat;
        }
        else
        {
            Body.GetComponent<Renderer>().material = freeMat;
            Head.GetComponent<Renderer>().material = freeMat;
        }
    }



    public void TurnOff()
    {
        spaceBlocked = 0;
        ChangeMaterial();
        gameObject.SetActive(false);
    }

    public void TurnOn()
    {    
        gameObject.SetActive(true);
        ChangeMaterial();
    }

    public void CreateTurret(Vector3 pos)
    {
        Instantiate(Turret, pos, Quaternion.identity);
    }
}
