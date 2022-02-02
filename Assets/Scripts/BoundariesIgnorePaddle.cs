using UnityEngine;

public class BoundariesIgnorePaddle : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Hitter").GetComponent<Collider>());
    }

}
