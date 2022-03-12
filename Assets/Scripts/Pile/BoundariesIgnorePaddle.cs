using UnityEngine;

public class BoundariesIgnorePaddle : MonoBehaviour
{
    void Start()
    {
        if(GameObject.Find("Gameplay")){
            Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Hitter").GetComponent<Collider>());
            Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Handle").GetComponent<Collider>());
        }
    }
}