using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<Transform> Nodes = new List<Transform>();

    public void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Nodes.Add(this.transform.GetChild(i));
        }
    }

    public void OnDrawGizmos()
    {
        //if (Nodes.Count > 0)
        //{
        //    Gizmos.color = Color.blue;

        //    //puntos
        //    for (int i = 0; i < Nodes.Count; i++)
        //    {
        //        Gizmos.DrawWireSphere(Nodes[i].position, 0.5f);
        //    }

        //    Gizmos.color = Color.black;

        //    //uniones
        //    for (int i = 0; i < Nodes.Count - 1; i++)
        //    {
        //        Gizmos.DrawLine(Nodes[i].position, Nodes[i + 1].position);
        //    }
        //}      
    }
}
