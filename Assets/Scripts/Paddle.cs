using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Touch currentTouch;

    bool isPaddle = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0){
            currentTouch = Input.GetTouch(0);

            //Raycasting at touch
            if(currentTouch.phase == TouchPhase.Began){
                Ray ray = Camera.main.ScreenPointToRay(currentTouch.position);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.gameObject.CompareTag("Paddle")){
                        //transform.Translate(Camera.main.ScreenToWorldPoint(Input.touches[0].position).x, transform.position.y, transform.position.z);
                        isPaddle = true;
                    }else{
                        isPaddle = false;
                    }
                }
            }

            if(isPaddle && currentTouch.phase == TouchPhase.Moved){
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(currentTouch.position).x, transform.position.y, transform.position.z);
            }
        }
    }
}
