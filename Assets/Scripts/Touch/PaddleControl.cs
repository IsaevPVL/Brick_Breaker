using UnityEngine;
using System;

public class PaddleControl : TouchableObject
{
    Rigidbody rb;

    [SerializeField, Range(0, 1)] float timeScaleMultiplyer = 0.2f;
    float fixedDeltaTime;

    public static event Action PaddleDoubleTapped;

    //TEMPORARY
    float widthMin = -5f;
    float widthMax = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (!isTouched)
        {
            SetTimeScale(timeScaleMultiplyer);
            return;
        }
        SetTimeScale(1);

        float boundX = Mathf.Clamp(touchPosition.x, widthMin, widthMax);

        Vector3 moveTo = new Vector3(boundX, objectDefaultPosition.y, objectDefaultPosition.z) + new Vector3(objectTouchOffset.x, 0f, 0f);

        rb.MovePosition(moveTo);

        if (objectDoubleTapped)
        {
            Debug.Log(name + " double tapped");
            PaddleDoubleTapped?.Invoke();
            objectDoubleTapped = false;
        }
    }

    void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
    }
}