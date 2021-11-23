using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;

    float moveX = 0f;
    public float speed = 15f;

    private void OnEnable() {
        PlayerInput.OnMove += Move;
    }
    private void OnDisable() {
        PlayerInput.OnMove -= Move;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        float positionX = rb.position.x + moveX * speed * Time.deltaTime;

        rb.MovePosition(new Vector3(positionX, rb.position.y, rb.position.z));
    }

    private void Move(float x){
        moveX = x;
    }
}
