using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRendererMeshTest : MonoBehaviour
{
    public Mesh mesh;
    void Start()
    {
        CanvasRenderer canvasRenderer = GetComponent<CanvasRenderer>();

        canvasRenderer.SetMesh(mesh);
    }

}
