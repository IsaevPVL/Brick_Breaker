using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTopologyTest : MonoBehaviour
{
    public GameObject obj;

    void Start()
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        //MeshTopology topology = mesh;
    }


}
