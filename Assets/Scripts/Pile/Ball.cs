using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public float contactMultiplier = 1f;
    public float speed = 10f;
    Rigidbody rb;
    bool isInUse;
    [Space, Header("Hit Particles")]
    public GameObject brickCollision;

    public static event Action DeathLineTouched;

    private void OnEnable()
    {
        PaddleControl.PaddleDoubleTapped += LaunchBall;
        ResourceManager.BallLoaded += DestroyThisBall;
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        PaddleControl.PaddleDoubleTapped -= LaunchBall;
        ResourceManager.BallLoaded -= DestroyThisBall;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Brick")){
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
        }
    }

    private void OnCollisionExit(Collision other)
    {
        rb.velocity = rb.velocity.normalized * speed;
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

    void DestroyThisBall(){
        if(isInUse)
            Destroy(this.gameObject);
    }
}