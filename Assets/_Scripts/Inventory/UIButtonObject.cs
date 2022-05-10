using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIButtonObject : PlaceableObject
{
    [Header("UI Button Properties")] 
    public UIState thisState;

    //bool isOpen = false;
    Color initialColor;
    Material material;
    Vector3 initialScale;
    UIManager manager;

    public override void OnEnable() {
        base.OnEnable();
        manager = GameObject.FindObjectOfType<UIManager>();
    }

    public override void Update()
    {
        //base.Update();

        if (isTapped)
        {
            //Debug.Log("Great Success");
            ResetTap();

            if (!manager.CheckCurrentState(thisState))
            {
                manager.SetThisStateCurrent(thisState);
                //PressButton();
            }
            else
            {
                manager.SetThisStateCurrent(UIState.Gameplay);
                //UnpressButton();
            }
        }
    }

    public void PressButton()
    {
        //isOpen = true;
        material = visual.GetComponent<MeshRenderer>().material;
        initialColor = material.color;
        material.color = Color.grey;

        initialScale = transform.localScale;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Max(0.1f, transform.localScale.z - 0.5f));
    }

    public void UnpressButton()
    {
        //isOpen = false;
        material.color = initialColor;
        transform.localScale = initialScale;
    }

    void SetDefaults(){

    }
}
