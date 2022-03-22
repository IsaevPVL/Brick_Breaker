using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float contactMultiplier = 1f;
    public float speed = 10f;
    
    public bool isCollidedAfterLaunch = false;

    [Space]
    public GameObject brickCollision;

    private void OnEnable()
    {
        PaddleControl.PaddleDoubleTapped += LaunchBall;

        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        PaddleControl.PaddleDoubleTapped -= LaunchBall;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCollidedAfterLaunch = true;

        if (collision.collider.CompareTag("Bottom"))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.collider.CompareTag("Hitter"))
        {
            CollideWithPaddle(collision);
        }

        if(collision.collider.CompareTag("Brick")){
            ContactPoint contact = collision.contacts[0];
            GameObject hit = Instantiate(brickCollision, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal));
            
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
}