using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform spawnPosition;
    Renderer meshRenderer;
    int health;


    private void Awake() {
        meshRenderer = GetComponent<Renderer>();
        health = Random.Range(1, 3);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ball")){
            Hit();
        }
    }

    void Hit(){
        health--;
        if(health <= 0){
            EnergyManager.active.AddEnergy(Random.Range(20, 41));
            Instantiate(projectile, spawnPosition.position, Quaternion.identity);
            Destroy(gameObject);
        }else{
            meshRenderer.material.color = Random.ColorHSV();
        }
    }
}
