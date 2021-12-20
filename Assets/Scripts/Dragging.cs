using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    float distanceZ;
    bool isDragging = false;
    Vector3 offset;
    Transform toDrag;
    Rigidbody rb;
    Camera cam;

    float width;
    float widthMin;
    float widthMax;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Start()
    {
        width = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        widthMin = -width / 2f + .8f;
        widthMax = width / 2f - .8f;
    }

    void Update()
    {
        Vector3 moveTo;

        if (Input.touchCount == 0)
        {
            isDragging = false;
        }
        else
        {
            Touch touch = Input.touches[0];
            Vector3 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Paddle")
                    {
                        toDrag = hit.transform;
                        distanceZ = hit.transform.position.z - Camera.main.transform.position.z;
                        //moveTo = new Vector3(touchPosition.x, touchPosition.y, distanceZ);
                        moveTo = new Vector3(touchPosition.x, 0, distanceZ);
                        moveTo = cam.ScreenToWorldPoint(moveTo);
                        offset = toDrag.position - moveTo;
                        isDragging = true;
                    }
                }
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                //moveTo = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceZ);
                moveTo = new Vector3(touchPosition.x, toDrag.position.y, distanceZ);
                moveTo = cam.ScreenToWorldPoint(moveTo) + offset;
                moveTo = new Vector3(Mathf.Clamp(moveTo.x, widthMin, widthMax), moveTo.y, moveTo.z);
                //toDrag.position = moveTo + offset;

                rb.MovePosition(moveTo);
            }

            if (isDragging && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
            {
                isDragging = false;
            }
        }
    }
}
