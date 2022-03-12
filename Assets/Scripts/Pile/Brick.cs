using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int health;

    Renderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ball")){
            Hit();
        }
    }

    void Hit(){
        health--;
        if(health <= 0){
            Destroy(gameObject);
        }else{
            meshRenderer.material.color = Random.ColorHSV();
        }
    }
}
