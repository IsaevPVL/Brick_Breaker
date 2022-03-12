using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public float contactMultiplier = 1f;
    public Rigidbody rb;
    public bool isCollidedAfterLaunch = false;

    public float speed = 10f;

    Vector3 lastVelocity;
    Vector3 direction;
    Vector3 normalSum;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(-collision.relativeVelocity);
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Debug.Log(contact.otherCollider.gameObject.name);
        // }
        isCollidedAfterLaunch = true;


        if (collision.gameObject.CompareTag("Bottom"))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.collider.CompareTag("Paddle"))
        {
            CollideWithPaddle(collision);
        }

        // if (collision.contactCount == 1)
        // {
        //     if (collision.collider.CompareTag("Paddle"))
        //     {
        //         CollideWithPaddle(collision);
        //     }
        //     else
        //     {
        //         CollideWithOne(collision);
        //     }
        // }
        // else
        // {
        //     CollideWithMultiple(collision);
        // }

        // Debug.Log("Collided to: " + direction);
    }


    private void OnCollisionStay(Collision collision)
    {
        // if(collision.gameObject.CompareTag("Boundary")){
        //     Vector3 correctedDirection = GameObject.FindWithTag("Paddle").transform.position - transform.position;
        //     NormalizeAndSetVelocity(correctedDirection);
        // }
    }

    private void OnCollisionExit(Collision other) {
        rb.velocity = rb.velocity.normalized * speed;
    }

    void CollideWithPaddle(Collision collision)
    {
        float paddleCentreX = collision.gameObject.transform.position.x;
        Vector3 collisionPoint = collision.GetContact(0).point;
        float distanceToCentre = paddleCentreX - collisionPoint.x;

        direction = new Vector3(-distanceToCentre * contactMultiplier, 1, 0);
        NormalizeAndSetVelocity(direction);

        Debug.Log("Collided with paddle");
    }

    void CollideWithOne(Collision collision)
    {
        if(collision.contactCount == 1){
            direction = Vector3.zero;
            lastVelocity = -collision.relativeVelocity;

            direction = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);
            NormalizeAndSetVelocity(direction);

            Debug.Log("Collided with one");
        }else{
            CollideWithMultiple(collision);
        }
    }

    void CollideWithMultiple(Collision collision)
    {
        direction = Vector3.zero;
        normalSum = Vector3.zero;
        lastVelocity = -collision.relativeVelocity;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            normalSum += collision.contacts[i].normal;
        }
        direction = normalSum;
        NormalizeAndSetVelocity(direction);

        Debug.Log("Collided with multiple");
    }

    void NormalizeAndSetVelocity(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
    }
}