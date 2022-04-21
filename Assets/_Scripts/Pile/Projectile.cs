using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    public int damage { get; private set; }

    private void Awake()
    {
        damage = Random.Range(10, 30);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hitter hitter))
        {
            hitter.TakeDamage(damage);
            Destroy(this.gameObject);
        }else if(other.CompareTag("Bottom")){
            Destroy(this.gameObject);
        }
    }
}
