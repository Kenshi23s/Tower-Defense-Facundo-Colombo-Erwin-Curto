using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildTowers : MonoBehaviour, IPausable
{
    public List<TowerSpaceDetect> MyTurrets = new List<TowerSpaceDetect>();

    public int ActualTurret = 0;

    Vector3 _pos;

    public LayerMask surface, layerObstacle, layerBlock;

    GameObject temp;

    [Range(0.1f, 20)]
    public float GridScale;

    bool CanBuild, Stoped = false, Onclicking;

    [Range(0.01f, 2)]
    public float CooldownTower;

    void Start()
    {
        ScreenManager.AddPausable(this);

        for (int i = 0; i < MyTurrets.Count; i++)
        {
            TowerSpaceDetect temp = Instantiate(MyTurrets[i],_pos,Quaternion.identity).GetComponent<TowerSpaceDetect>();
            MyTurrets[i] = temp;

            MyTurrets[i].TurnOff();
        }
    }

    void Update()
    {
        if (!Stoped)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnPushDown();
                OnPush();
                Onclicking = true;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                OnPush();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                OnPushUp();
                Onclicking = false;
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                ChangeTurretSelected(0);
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                ChangeTurretSelected(1);
            }

            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                ChangeTurretSelected(2);
            }
        }
    }

    public bool GetMouseInWorld(LayerMask colisionlayerclick, LayerMask obstaclelayer, out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 9999, colisionlayerclick))
        {

            if (!Physics.Raycast(ray, hit.distance, obstaclelayer))
            {
                point = hit.point;

                return true;
            }
        }


        point = Vector3.zero;

        return false;
    }

    public Vector3 MoveToGrid(Vector3 pos, float gridScale)
    {
        pos = (pos / gridScale);

        pos = new Vector3((int)pos.x, (int)pos.y, (int)pos.z);

        pos = gridScale * pos;

        return pos;
    }

    IEnumerator BuildCoolDownTime()
    {
        CanBuild = false;
        yield return new WaitForSeconds(CooldownTower);

        CanBuild = true;
        yield return null;
    }

    #region Pausable
    public void Pause()
    {
        Stoped = true;
    }

    public void Resume()
    {
        Stoped = false;
        MyTurrets[ActualTurret].TurnOff();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (GetMouseInWorld(surface, layerBlock, out _pos))
            {
                MyTurrets[ActualTurret].TurnOn();
                _pos = MoveToGrid(_pos, GridScale);

            }

            StartCoroutine(BuildCoolDownTime());
        }
    }

    #endregion

    #region ClickingInMap
    public void OnPushDown()
    {
        if (GetMouseInWorld(surface, layerBlock, out _pos))
        {
            MyTurrets[ActualTurret].TurnOn();
            _pos = MoveToGrid(_pos, GridScale);

        }

        StartCoroutine(BuildCoolDownTime());
    }

    public void OnPush()
    {
        if (GetMouseInWorld(surface, layerBlock, out _pos))
        {
            if (MyTurrets[ActualTurret].gameObject.activeSelf == false)
            {
                MyTurrets[ActualTurret].TurnOn();
            }

            _pos = MoveToGrid(_pos, GridScale);
            MyTurrets[ActualTurret].transform.position = _pos;
        }

    }

    public void OnPushUp()
    {
        if (GetMouseInWorld(surface, layerBlock, out _pos))
        {
            MyTurrets[ActualTurret].transform.position = _pos;

            if (GetMouseInWorld(surface, layerBlock, out _pos) && MyTurrets[ActualTurret].spaceBlocked <= 0 && CanBuild)
            {
                _pos = MoveToGrid(_pos, GridScale);
                MyTurrets[ActualTurret].CreateTurret(_pos);
            }          
        }

        MyTurrets[ActualTurret].TurnOff();
    }

    #endregion

    public void ChangeTurretSelected(int CambiarArmaActual)
    {
        ActualTurret = CambiarArmaActual;

        for (int i = 0; i < MyTurrets.Count; i++)
        {
            if (MyTurrets[i] != null)
            {
                if (i == ActualTurret && Onclicking)
                {
                    MyTurrets[i].TurnOn();
                }
                else
                {
                    MyTurrets[i].TurnOff();
                }
            }
           
        }
    }
}
