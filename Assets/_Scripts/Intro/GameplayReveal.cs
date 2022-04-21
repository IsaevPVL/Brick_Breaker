using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayReveal : MonoBehaviour
{
    public Texture2D frame;

    private void OnEnable() {
        frame = GameObject.Find("Frame").GetComponent<Frame>().frame;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_MainTex", frame);
    }
}
