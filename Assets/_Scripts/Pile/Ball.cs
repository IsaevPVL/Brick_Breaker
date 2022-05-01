using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public float contactMultiplier = 1f;
    public float speed = 10f;
    public Rigidbody rb;
    bool isInUse;
    [Space, Header("Hit Particles")]
    public GameObject brickCollision;

    public Transform visual;
    //Vector3 fromRotation;

    public static event Action DeathLineTouched;

    private void OnEnable()
    {
        PaddleControl.PaddleDoubleTapped += LaunchBall;
        HealthManager.BallLoaded += DestroyThisBall;
        rb = GetComponent<Rigidbody>();


    }

    private void OnDisable()
    {
        PaddleControl.PaddleDoubleTapped -= LaunchBall;
        HealthManager.BallLoaded -= DestroyThisBall;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(rb.velocity);

        if (collision.collider.CompareTag("Brick"))
        {
            ContactPoint contact = collision.contacts[0];
            GameObject hit = Instantiate(brickCollision, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal));
        }
        else if (collision.collider.CompareTag("Hitter"))
        {
            CollideWithPaddle(collision);
        }
        else if (collision.collider.CompareTag("Bottom"))
        {
            DeathLineTouched?.Invoke();
            Destroy(this.gameObject);
            return;
        }
        RotateVisual();
    }

    private void OnCollisionExit(Collision other)
    {
        //Preventing ball from staying on X/Y axis
        if (rb.velocity.x < 0.001 && rb.velocity.x > -0.001)
        {
            rb.velocity += Vector3.right * UnityEngine.Random.Range(-0.1f, 0.1f);
        }
        if (rb.velocity.y < 0.001 && rb.velocity.y > -0.001)
        {
            rb.velocity += Vector3.up * UnityEngine.Random.Range(-0.1f, 0.1f);
        }

        rb.velocity = rb.velocity.normalized * speed;


    }

    void RotateVisual(){
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f;
        visual.rotation = Quaternion.Euler(0, 0, angle);
    }

    void LaunchBall()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
        isInUse = true;
    }

    void CollideWithPaddle(Collision collision)
    {
        float paddleCentreX = collision.gameObject.transform.position.x;
        Vector3 collisionPoint = collision.GetContact(0).point;
        float distanceToCentre = paddleCentreX - collisionPoint.x;

        Vector3 direction = new Vector3(-distanceToCentre * contactMultiplier, 1, 0);
        NormalizeAndSetVelocity(direction);
    }

    void NormalizeAndSetVelocity(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    void DestroyThisBall()
    {
        if (isInUse)
            Destroy(this.gameObject);
    }
}