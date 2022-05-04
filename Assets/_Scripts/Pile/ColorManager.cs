using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class ColorManager : MonoBehaviour
{
    [SerializeField] Color black;
    [SerializeField] Color white;
    [SerializeField] Color red;


    [Space, Header("Materials")]
    [SerializeField] Material blackMaterial;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material redMaterial;


    [Space, Header("Text")]
    [SerializeField] Color textColor1;
    [SerializeField] TextMeshPro[] textToColor1;
    [SerializeField] Color textColor2;
    [SerializeField] TextMeshPro[] textToColor2;

    [ContextMenu("Color Objects")]
    void ColorObjects()
    {
        Camera.main.backgroundColor = black;

        LineRenderer deathLine = GameObject.FindGameObjectWithTag("Bottom").GetComponent<LineRenderer>();
        deathLine.startColor = red;
        deathLine.endColor = red;

        blackMaterial.color = black;
        whiteMaterial.color = white;
        redMaterial.color = red;

        foreach (TextMeshPro text in textToColor1)
        {
            text.color = textColor1;
        }

        foreach (TextMeshPro text in textToColor2)
        {
            text.color = textColor2;
        }
    }
}
