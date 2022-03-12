using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingTouchManager : MonoBehaviour
{   
    TouchManager tm;
    Rigidbody rb;
    public bool isTouched = false;

    private void OnEnable() {
        TouchManager.ObjectWasTouched += ThisObject;
        TouchManager.TouchEnded += TouchEnded;
    }

    private void OnDisable() {
        TouchManager.ObjectWasTouched -= ThisObject;
        TouchManager.TouchEnded -= TouchEnded;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        tm = TouchManager.active;
    }

    void ThisObject(GameObject obj){
        if(obj == this.gameObject){
            Debug.Log(this.name);
            isTouched = true;
        }
    }

    void TouchEnded(){
        isTouched = false;
    }

    private void Update() {
        if(isTouched && tm.touchMoved){
            Vector3 position = tm.currentTouchPosition + new Vector3(tm.objectTouchOffset.x, tm.objectTouchOffset.y, 0f);
            rb.MovePosition(position);
        }
    }
}
